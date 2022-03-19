# **ExpiryDate**

City builder game set during a climatic calamity with a hint of fantasy, created in a team of ten as part of a Polish game development championship - [Turniej Trójgamiczny](https://www.t3g.pl/).

## Table of Contents

- [Table of Contents](#table-of-contents)
- [**Authors**](#authors)
- [**Technologies Used**](#technologies-used)
- [**Developer Notes**](#developer-notes)
  - [**Git Notes**](#git-notes)
- [**Mechanics**](#mechanics)
  - [**Custon scriptable objects creation (battle system)**](#Custom-scriptable-objects-creation)

## **Authors**

- **Game development path:**
  - Damian Dorosz (team leader)
  - Jakub Jakubowski
  - Michał Cebula
  - Bartosz Podgórski
  - ~~Bartosz Kapusta~~
  - ~~Adam Strzelczyk~~
 
- **Game design path:**
  - Agata Musiał
  - Jakub Sadowski
  - ~~Filip Łatka~~
  
- **Experience design path:**
  - Olga Ślifirczyk
  - Maciej Trzmiel
  - Bartłomiej Kazirodek
  
## **Technologies Used**

- Unity 2020.3.25f1

## **Developer Notes**

Make many <u>small</u> **Pull Requests** rather than packing too much code into one branch.

We want every prefab or asset to be able to be used in any scene. If there are steps needed to get it working (initializing variables, etc), document them here under the [Mechanics](#mechanics) section.

Because of the way `.meta` files work, we should never commit an empty folder to source control. If we do, every other developer will have conflicts.

### **Git Notes**

We are using Github Projects to track issues, The link is found [here](https://github.com/exostin/ExpiryDate/projects/1). Each ticket is automatically given a number. When you start working on an issue, create a feature branch in git with the following syntax. You'll also need to assign yourself to the issue in the `Issues` tab.

``` git
git checkout -b x-short-description-of-issue
git push -u origin x-short-description-of-issue

x - issue number
```

At this point, the branch is both local and is tracked in your fork. You can then freely make changes to the branch and the `main` branch will be unaffected.

Once you're done working on an issue submit a pull request, and link it to that issue so it can be automatically closed after merging.

![linking pull request to an issue](https://i.ibb.co/JpyX08X/Link-Pull-Request-To-Issue-Example.png)

## **Mechanics**

### **Custom scriptable objects creation**

1. Right click in the destination folder
2. Press Create
3. Choose what object you want to create (Character/Ability)

![scriptable objects creation image](https://i.ibb.co/hRTgWT7/Custom-Scriptable-Objects-Creation.jpg)

4. Once you create your object, head on to Inspector to modify it's values (Character name, health etc.)

![ScriptableObject inspector image](https://i.ibb.co/b6r1Wj1/Scriptable-Object-Inspector.jpg)
