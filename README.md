# **ExpiryDate**

City building game about climate change, which we are doing for a polish game making tournament ([Turniej Trójgamiczny](https://www.t3g.pl/)).

## Table of Contents

- [Table of Contents](#table-of-contents)
- [**Authors**](#authors)
- [**Technologies Used**](#technologies-used)
- [**Developer Notes**](#developer-notes)
  - [**Git Notes**](#git-notes)
- [**Mechanics**](#mechanics)
  - [**Example of a well documented mechanic**](#highlight-mechanic)

## **Authors**

- **Game development path:**
  - Damian Dorosz
  - Bartosz Podgórski
  - Bartosz Kapusta
  - Adam Strzelczyk
- **Game design path:**
  - Jakub Sadowski
  - Filip Łatka
  - Adam Musiał
- **Experience design path:**
  - Olga Ślifirczyk
  - Maciej Trzmiel
  - Bartłomiej Kazirodek
  
## **Technologies Used**

- Unity 2020.3.25
- ...

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

---

### ***Example of a well documented mechanic:***

---

### **Highlight Mechanic**

#### **How to use**

1. Attach this script to the `GameObject` you want to highlight.

2. Create a highlight prefab and assign it to the `GameObject` like so. Keep clone empty.

![prefab image](https://i.ibb.co/fY3Rbrt/Edited-Grid-Generator.png)

- `Main Cam`: The camera, whereby the player sees the scene
- `Player`: The y-component if the position of this object will be used when generating the grid and instantiate the  `highlight` object
- `Take Object Transform`: An option to take the position of the game object, which has the script, as the start position
- `Destroy`: An option to destroy the `highlight` objects
- `Gridstart` The start position of the grid(lower right corner)
- `GridSize`: The size of the grid
- `Layer`: Every object with this layer will be ignored
- `SelectionKey`: The button that must be pressed so that the selected tiles are not destroyed
- `Clear SelectionKey`: The button that muss be pressed to delete all selected tiles

Note: The y-value of the `Gridstart` variable will be ignored, because we using the y-value of the `Player`s position, if the `take object transform` option is false

Note: In sone function, you has to use a argument of the type `TypesofValue`. This is a enum set, which contains two values:

- `relative`: the instantiated `highlight` objects adapt to the rotation of the player
- `absolute`: the instantiated `highlight` object wont adapt to the rotation of the player

#### **How it works**

Once the scene is started, a grid is created from EditedInvisGridTile objects. Now the script checks whether the player presses a certain mouse button. If this is the case, a raycast is sent from the camera. As soon as this raycast hits an object, it checks whether it is an "EditedInvisGridTile" object. If so, a clone of the 'Hightlight' object is instantiated to the position of the EditedInvisGridTile object.

In this case, I am cloning a quad that is emissive (looks like a highlight).

![highlight image](https://i.ibb.co/6vX1CkF/Screenshot-2020-12-05-144732.png)
