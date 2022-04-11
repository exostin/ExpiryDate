/*
 * Copyright (c) 2019 Made With Monster Love (Pty) Ltd
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the 
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR 
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonsterLove.StateMachine
{
    public enum StateTransition
    {
        Safe,
        Overwrite
    }

    public interface IStateMachine<TDriver>
    {
        MonoBehaviour Component { get; }
        TDriver Driver { get; }
        bool IsInTransition { get; }
    }

    public class StateMachine<TState> : StateMachine<TState, StateDriverRunner>
        where TState : struct, IConvertible, IComparable
    {
        public StateMachine(MonoBehaviour component) : base(component)
        {
        }
    }

    public class StateMachine<TState, TDriver> : IStateMachine<TDriver> where TState : struct, IConvertible, IComparable
        where TDriver : class, new()
    {
        private static readonly BindingFlags bindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private StateMapping<TState, TDriver> currentState;
        private IEnumerator currentTransition;
        private StateMapping<TState, TDriver> destinationState;
        private IEnumerator enterRoutine;
        private readonly Func<TState, int> enumConverter;
        private IEnumerator exitRoutine;

        private StateMapping<TState, TDriver> lastState;
        private IEnumerator queuedChange;
        private StateMapping<TState, TDriver> queuedState;

        public bool reenter = false;

        private readonly Dictionary<object, StateMapping<TState, TDriver>> stateLookup;
        public event Action<TState> Changed;

        #region Initialization

        public StateMachine(MonoBehaviour component)
        {
            Component = component;

            //Compiler shenanigans to get ints from generic enums
            Func<int, int> identity = Identity;
            enumConverter = Delegate.CreateDelegate(typeof(Func<TState, int>), identity.Method) as Func<TState, int>;

            //Define States
            var enumValues = Enum.GetValues(typeof(TState));
            if (enumValues.Length < 1)
                throw new ArgumentException("Enum provided to Initialize must have at least 1 visible definition");

            var enumBackingType = Enum.GetUnderlyingType(typeof(TState));
            if (enumBackingType != typeof(int))
                throw new ArgumentException("Only enums with an underlying type of int are supported");

            //Find all items in Driver class
            // public class Driver
            // {
            //     StateEvent Foo;        <- Selected
            //     StateEvent<int> Boo;   <- Selected 
            //     float x;               <- Throw exception
            // }
            var eventFields = GetFilteredFields(typeof(TDriver), "MonsterLove.StateMachine.StateEvent");
            var eventFieldsLookup = CreateFieldsLookup(eventFields);

            //Instantiate driver
            // driver = new Driver();
            // for each StateEvent:
            //   StateEvent foo = new StateEvent(isAllowed, getStateInt, capacity);
            Driver = CreateDriver(IsDispatchAllowed, GetStateInt, enumValues.Length, eventFields);

            // Create a state mapping for each state defined in the enum
            stateLookup = CreateStateLookup(this, enumValues);

            //Collect methods in target component
            var methods = component.GetType().GetMethods(bindingFlags);

            //Bind methods to states
            for (var i = 0; i < methods.Length; i++)
            {
                TState state;
                string evtName;
                if (!ParseName(methods[i], out state, out evtName))
                    continue; //Skip methods where State_Event name convention could not be parsed

                var mapping = stateLookup[state];

                if (eventFieldsLookup.ContainsKey(evtName))
                {
                    //Bind methods defined in TDriver
                    // driver.Foo.AddListener(StateOne_Foo);
                    var eventField = eventFieldsLookup[evtName];
                    BindEvents(Driver, component, state, enumConverter(state), methods[i], eventField);
                }
                else
                {
                    //Bind Enter, Exit and Finally Methods
                    BindEventsInternal(mapping, component, methods[i], evtName);
                }
            }

            //Create nil state mapping
            currentState = null;
        }

        private static List<FieldInfo> GetFilteredFields(Type type, string searchTerm)
        {
            var list = new List<FieldInfo>();

            var fields = type.GetFields(bindingFlags);

            for (var i = 0; i < fields.Length; i++)
            {
                var item = fields[i];
                if (item.FieldType.ToString().Contains(searchTerm))
                    list.Add(item);
                else
                    throw new ArgumentException(
                        string.Format("{0} contains unsupported type {1}", type, item.FieldType));
            }

            return list;
        }

        private static Dictionary<string, FieldInfo> CreateFieldsLookup(List<FieldInfo> fields)
        {
            var dict = new Dictionary<string, FieldInfo>();

            for (var i = 0; i < fields.Count; i++)
            {
                var item = fields[i];

                dict.Add(item.Name, item);
            }

            return dict;
        }

        private static Dictionary<object, StateMapping<TState, TDriver>> CreateStateLookup(
            StateMachine<TState, TDriver> fsm, Array values)
        {
            var stateLookup = new Dictionary<object, StateMapping<TState, TDriver>>();
            for (var i = 0; i < values.Length; i++)
            {
                var mapping = new StateMapping<TState, TDriver>(fsm, (TState) values.GetValue(i), fsm.GetState);
                stateLookup.Add(mapping.state, mapping);
            }

            return stateLookup;
        }

        private static TDriver CreateDriver(Func<bool> isInvokeAllowedCallback, Func<int> getStateIntCallback,
            int capacity, List<FieldInfo> fieldInfos)
        {
            if (fieldInfos == null)
                throw new ArgumentException(string.Format("Arguments cannot be null. Callback {0} fieldInfos {1}",
                    isInvokeAllowedCallback, fieldInfos));

            var driver = new TDriver();

            for (var i = 0; i < fieldInfos.Count; i++)
            {
                //driver.Event = new StateEvent(callback)
                var fieldInfo = fieldInfos[i]; //Event
                var constructorInfo =
                    fieldInfo.FieldType.GetConstructor(new[]
                    {
                        typeof(Func<bool>), typeof(Func<int>), typeof(int)
                    }); //StateEvent(Func<Bool> invokeAllowed, Func<in> getState, int capacity)
                var obj = constructorInfo.Invoke(new object[]
                {
                    isInvokeAllowedCallback, getStateIntCallback, capacity
                }); //obj = new StateEvent(Func<bool> isInvokeAllowed, Func<int> stateProvider, int capacity);
                fieldInfo.SetValue(driver, obj); //driver.Event = obj;
            }

            return driver;
        }

        private static bool ParseName(MethodInfo methodInfo, out TState state, out string eventName)
        {
            state = default;
            eventName = null;

            if (methodInfo.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length != 0) return false;

            var name = methodInfo.Name;
            var index = name.IndexOf('_');

            //Ignore functions without an underscore
            if (index < 0) return false;

            var stateName = name.Substring(0, index);
            eventName = name.Substring(index + 1);

            try
            {
                state = (TState) Enum.Parse(typeof(TState), stateName);
            }
            catch (ArgumentException)
            {
                //Not an method as listed in the state enum
                return false;
            }

            return true;
        }

        private static void BindEvents(TDriver driver, Component component, TState state, int stateInt,
            MethodInfo stateTargetDef, FieldInfo driverEvtDef)
        {
            var genericTypes =
                driverEvtDef.FieldType.GetGenericArguments(); //get T1,T2,...TN from StateEvent<T1,T2,...TN>
            var actionType = GetActionType(genericTypes); //typeof(Action<T1,T2,...TN>)

            //evt.AddListener(State_Method); 
            var obj = driverEvtDef.GetValue(driver); //driver.Foo
            var addMethodInfo = driverEvtDef.FieldType.GetMethod("AddListener", bindingFlags); // driver.Foo.AddListener

            Delegate del = null;
            try
            {
                del = Delegate.CreateDelegate(actionType, component, stateTargetDef);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException(string.Format(
                    "State ({0}_{1}) requires a callback of type: {2}, type found: {3}", state, driverEvtDef.Name,
                    actionType, stateTargetDef));
            }

            addMethodInfo.Invoke(obj,
                new object[] {stateInt, del}); //driver.Foo.AddListener(stateInt, component.State_Event);
        }

        private static void BindEventsInternal(StateMapping<TState, TDriver> targetState, Component component,
            MethodInfo method, string evtName)
        {
            switch (evtName)
            {
                case "Enter":
                    if (method.ReturnType == typeof(IEnumerator))
                    {
                        targetState.hasEnterRoutine = true;
                        targetState.EnterRoutine = CreateDelegate<Func<IEnumerator>>(method, component);
                    }
                    else
                    {
                        targetState.hasEnterRoutine = false;
                        targetState.EnterCall = CreateDelegate<Action>(method, component);
                    }

                    break;
                case "Exit":
                    if (method.ReturnType == typeof(IEnumerator))
                    {
                        targetState.hasExitRoutine = true;
                        targetState.ExitRoutine = CreateDelegate<Func<IEnumerator>>(method, component);
                    }
                    else
                    {
                        targetState.hasExitRoutine = false;
                        targetState.ExitCall = CreateDelegate<Action>(method, component);
                    }

                    break;
                case "Finally":
                    targetState.Finally = CreateDelegate<Action>(method, component);
                    break;
            }
        }

        private static V CreateDelegate<V>(MethodInfo method, object target) where V : class
        {
            var ret = Delegate.CreateDelegate(typeof(V), target, method) as V;

            if (ret == null) throw new ArgumentException("Unable to create delegate for method called " + method.Name);

            return ret;
        }

        private static Type GetActionType(Type[] genericArgs)
        {
            switch (genericArgs.Length)
            {
                case 0:
                    return typeof(Action);
                case 1:
                    return typeof(Action<>).MakeGenericType(genericArgs);
                case 2:
                    return typeof(Action<,>).MakeGenericType(genericArgs);
                default:
                    throw new ArgumentOutOfRangeException(
                        string.Format("Cannot create Action Type with {0} type arguments", genericArgs.Length));
            }
        }

        #endregion

        #region ChangeStates

        public void ChangeState(TState newState)
        {
            ChangeState(newState, StateTransition.Safe);
        }

        public void ChangeState(TState newState, StateTransition transition)
        {
            if (stateLookup == null)
                throw new Exception(
                    "States have not been configured, please call initialized before trying to set state");

            if (!stateLookup.ContainsKey(newState))
                throw new Exception("No state with the name " + newState +
                                    " can be found. Please make sure you are called the correct type the statemachine was initialized with");

            var nextState = stateLookup[newState];

            if (!reenter && currentState == nextState) return;

            //Cancel any queued changes.
            if (queuedChange != null)
            {
                Component.StopCoroutine(queuedChange);
                queuedChange = null;
            }

            switch (transition)
            {
                //case StateMachineTransition.Blend:
                //Do nothing - allows the state transitions to overlap each other. This is a dumb idea, as previous state might trigger new changes. 
                //A better way would be to start the two couroutines at the same time. IE don't wait for exit before starting start.
                //How does this work in terms of overwrite?
                //Is there a way to make this safe, I don't think so? 
                //break;
                case StateTransition.Safe:
                    if (IsInTransition)
                    {
                        if (exitRoutine !=
                            null) //We are already exiting current state on our way to our previous target state
                        {
                            //Overwrite with our new target
                            destinationState = nextState;
                            return;
                        }

                        if (enterRoutine !=
                            null) //We are already entering our previous target state. Need to wait for that to finish and call the exit routine.
                        {
                            //Damn, I need to test this hard
                            queuedChange = WaitForPreviousTransition(nextState);
                            Component.StartCoroutine(queuedChange);
                            return;
                        }
                    }

                    break;
                case StateTransition.Overwrite:
                    if (currentTransition != null) Component.StopCoroutine(currentTransition);

                    if (exitRoutine != null) Component.StopCoroutine(exitRoutine);

                    if (enterRoutine != null) Component.StopCoroutine(enterRoutine);

                    //Note: if we are currently in an EnterRoutine and Exit is also a routine, this will be skipped in ChangeToNewStateRoutine()
                    break;
            }


            if (currentState != null && currentState.hasExitRoutine || nextState.hasEnterRoutine)
            {
                IsInTransition = true;
                currentTransition = ChangeToNewStateRoutine(nextState, transition);
                Component.StartCoroutine(currentTransition);
            }
            else //Same frame transition, no coroutines are present
            {
                destinationState = nextState; //Assign here so Exit() has a valid reference

                if (currentState != null)
                {
                    currentState.ExitCall();
                    currentState.Finally();
                }

                lastState = currentState;
                currentState = destinationState;
                if (currentState != null)
                {
                    currentState.EnterCall();
                    if (Changed != null) Changed(currentState.state);
                }

                IsInTransition = false;
            }
        }

        private IEnumerator ChangeToNewStateRoutine(StateMapping<TState, TDriver> newState, StateTransition transition)
        {
            destinationState = newState; //Cache this so that we can overwrite it and hijack a transition

            if (currentState != null)
            {
                if (currentState.hasExitRoutine)
                {
                    exitRoutine = currentState.ExitRoutine();

                    if (exitRoutine != null &&
                        transition != StateTransition.Overwrite) //Don't wait for exit if we are overwriting
                        yield return Component.StartCoroutine(exitRoutine);

                    exitRoutine = null;
                }
                else
                {
                    currentState.ExitCall();
                }

                currentState.Finally();
            }

            lastState = currentState;
            currentState = destinationState;

            if (currentState != null)
            {
                if (currentState.hasEnterRoutine)
                {
                    enterRoutine = currentState.EnterRoutine();

                    if (enterRoutine != null) yield return Component.StartCoroutine(enterRoutine);

                    enterRoutine = null;
                }
                else
                {
                    currentState.EnterCall();
                }

                //Broadcast change only after enter transition has begun. 
                if (Changed != null) Changed(currentState.state);
            }

            IsInTransition = false;
        }

        private IEnumerator WaitForPreviousTransition(StateMapping<TState, TDriver> nextState)
        {
            queuedState = nextState; //Cache this so fsm.NextState is accurate;

            while (IsInTransition) yield return null;

            queuedState = null;
            ChangeState(nextState.state);
        }

        #endregion

        #region Properties & Helpers

        public bool LastStateExists => lastState != null;

        public TState LastState
        {
            get
            {
                if (lastState == null)
                    throw new NullReferenceException(
                        "LastState cannot be accessed before ChangeState() has been called at least twice");

                return lastState.state;
            }
        }

        public TState NextState
        {
            get
            {
                if (queuedState !=
                    null) //In safe mode sometimes we need to wait for the destination state to complete, and will be stored in queued state
                    return queuedState.state;

                if (destinationState == null) return State;

                return destinationState.state;
            }
        }

        public TState State
        {
            get
            {
                if (currentState == null)
                    throw new NullReferenceException(
                        "State cannot be accessed before ChangeState() has been called at least once");

                return currentState.state;
            }
        }

        public bool IsInTransition { get; private set; }

        public TDriver Driver { get; }

        public MonoBehaviour Component { get; }

        //format as method so can be passed as Func<TState>
        private TState GetState()
        {
            return State;
        }

        private int GetStateInt()
        {
            return enumConverter(State);
        }

        //Compiler shenanigans to get ints from generic enums
        private static int Identity(int x)
        {
            return x;
        }

        private bool IsDispatchAllowed()
        {
            if (currentState == null) return false;

            if (IsInTransition) return false;

            return true;
        }

        #endregion

        #region Static API

        //Static Methods

        /// <summary>
        ///     Inspects a MonoBehaviour for state methods as defined by the supplied Enum, and returns a stateMachine instance
        ///     used to transition states.
        /// </summary>
        /// <param name="component">The component with defined state methods</param>
        /// <returns>A valid stateMachine instance to manage MonoBehaviour state transitions</returns>
        public static StateMachine<TState> Initialize(MonoBehaviour component)
        {
            var engine = component.GetComponent<StateMachineRunner>();
            if (engine == null) engine = component.gameObject.AddComponent<StateMachineRunner>();

            return engine.Initialize<TState>(component);
        }

        /// <summary>
        ///     Inspects a MonoBehaviour for state methods as defined by the supplied Enum, and returns a stateMachine instance
        ///     used to transition states.
        /// </summary>
        /// <param name="component">The component with defined state methods</param>
        /// <param name="startState">The default starting state</param>
        /// <returns>A valid stateMachine instance to manage MonoBehaviour state transitions</returns>
        public static StateMachine<TState> Initialize(MonoBehaviour component, TState startState)
        {
            var engine = component.GetComponent<StateMachineRunner>();
            if (engine == null) engine = component.gameObject.AddComponent<StateMachineRunner>();

            return engine.Initialize(component, startState);
        }

        #endregion
    }
}