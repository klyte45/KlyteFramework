# Klyte's Framework
This mod is a basic tool for asset makers that want to use simple features from Klyte Commons Includable framework (5th gen mods).
Currently it suppots the features listed below:

## I18n engine

Any asset can create a new entry in the game localization file by simple adding a folder called `kf_translationFiles` in the workshop export folder. The file format is the same from Commons Includable, and support the languages listed there.
By default, all entries are prefixed with `K45_` string. To avoid it (to override a game default locale entry) the entry should start with a `%` symbol, like described in the Commons Includable's main page.

## Texture atlas engine (NEW IN 1.1)

### Per asset file addition

Any asset can create new items inside game default texture atlas, for use in UI as example. Add a folder called `kf_imageFiles` in the workshop export folder and put all images in **PNG format** inside.
By default, all sprite entries are prefixed with `K45_PACK.{workshopId}_`, this is used to prevent collision between asset images. If you **REALLY** want to use the name as is, put a `%` symbol in the start of the name.


#### Image borders configurations 
If necessary you can add a file in the folder called `bordersDescriptor.txt` to describe the borders of each sprite. The file format is:

```json
{filenameWithoutExtension}={left},{right},{top},{bottom}
```

Use one line per image file. The border descriptor is used to tell the game which part of the image will not be rescaled when using a greater dimension. (Useful for panels)

### Local sprites

Additionally, you can add your own sprites in a local folder for personal use. It's located in the path `{CS_APPDATA_FOLDER}/Klyte45Mods/kf_imageFiles`. By default, all sprite names are prefixed with `K45_KF_LOCAL_`, but can be escaped starting with a `%` as well.

### Export game sprites

All game sprites can be exported clicking the button `Export game images` in mod options panel in pause/options menu.
All files are exported in a compatible format to be reimported into the game (name escaped + borders descriptors) and all them are exported to folder `{CS_APPDATA_FOLDER}/Klyte45Mods/kf_exportedAtlas`.

***WARNING: THIS FOLDER IS ERASED BEFORE EVERY EXPORT!*** So make sure to put your edited files into the import folder or in another safe place to prevent losing your work...


## Text-in-inline-texture (NEW IN 1.1)

Now is also possible to draw a sprite with some text inside it inside a text in the game UI, like the TLM line icons. Use the following tag in your text:
 ```xml
<k45LineSymbol {spriteName},{bgcolorRGBnoHashtag},{textToWrite}>
```

Note that there's no space between the commas. The UI item must be allowed to process markups to this work!
Avoid using more than 7 characters in the text...

### More features may be added in the future...
