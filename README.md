# PURR Unity RPG Resources

Toolkit enabling approachable workflow for making games in Unity:

1. Define maps, entities and properties in Tiled
2. Compose components on prefabs in Unity.
3. Yep.

## Assets Folder structure

* **Examples**/
  * **Platformer**/ (sidescroller)
  * **Roguelike**/ (topdown gridlocked walking sim)
    * **Prefabs**/ (instantiated from tilemap import)
    * **Map.tmx**
    * **Spritesheet.tsx** (characters)
    * **Tileset.tsx** (map tiles)
  * **Animalese.wav** (sound font for textbox)
  * **m5x7.asset** (TextMesh Pro font asset)
  * **UI.png** (shared UI sprites)
* **Gizmos**/ (component icons)
* **Plugins**/
  * **Generated**/ (automatically generated plugins)
    * **Lysing.dll** (compiled textbox grammar parser)
  * **GitHub**/ (GitHub integration for unity)
  * **SerializableDictionary**/ (for `PropertyMap`)
  * **TextMesh Pro/** (teletype effect)
  * **UnityAsync**/ (frame support for `async`/`await`)
  * **Hime.Redist.dll** (runtime parser classes)
  * **Hime.SDK.dll** (compile time parser generator)
* **PURR**/ (main project)
  * **Components**/ (userland behaviour, see below)
  * **Core**/
    * *Classes**/ (current representation of Tiled objects and tiles)
    * *Components**/ (non-prefab components)
    * *Extensions**/ (useful extensions to built-in types)
    * *Tools**/ (custom serializable classes)
    * *Utils**/ (useful utilities when writing components)
  * **Editor**/
    * **Grammar**/ (Hime grammar importer, and Lýsing grammar definition)
    * **Helpers**/
	* **Tiled**/ (tilemap and -set importers and classes)
  * **Parsers**/ (textbox-related stuff)
  * **ControllerInputModule.cs** (standard input module which discards click-selection)
  * **PURR.asmdef** (project assembly definition, references required assemblies)


## Components

Described on the [wiki](https://github.com/EmmaEwert/PURR/wiki/Components)


## Text

The built-in text system uses Lýsing, a custom format inspired by Earthbound's text engine.

	Plaintext is written as is, [red]with markers for [color], [pause]s, prompts, screenshakes, and
	other pragmatics

Currently supported:

* `[red]`, `[green]`, and other colors defined in the [Unity Manual entry for Rich Text]
* `[color]` to reset the color to its standard value
* `[pause]` to wait for user input before continuing


[Unity Manual entry for Rich Text]: https://docs.unity3d.com/Manual/StyledText.html