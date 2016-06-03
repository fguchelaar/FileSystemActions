FileSystemActions
=================

Provides a configurable Windows Service leveraging the FileSystemWatcher, to perform actions when a directory or file is created, changed, renamed or deleted.

#Configuration example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<watchers>
  <watcher path="c:\Temp">
    <action event="onCreated" command="onCreated.bat" runOnStartup="true" />
    <action event="onChanged" command="onChanged.bat" />
  </watcher>
  <watcher path="c:\Temp">
    <action event="onChanged" command="onChangedAgain.bat" />
  </watcher>
  <watcher path="c:\Temp" filter="readme.*">
    <action event="onRenamed" command="onRenamed.bat" />
    <action event="onDeleted" command="onDeleted.bat" runOnStartup="true" />
  </watcher>
  <watcher path="c:\Temp" filter="readme.*" timeout="5000">
    <action event="onRenamed" command="onRenamed.bat" />
    <action event="onDeleted" command="onDeleted.bat" />
  </watcher>
  <watcher path="c:\Temp" filter="readme.*" timeout="5000" debounceOnFolder="true">
    <action event="onRenamed" command="onRenamed.bat" />
    <action event="onDeleted" command="onDeleted.bat" />
  </watcher>
</watchers>
```

When using the (optional) `timeout` attribute, the actions will not get called multiple times if an event occurs within that timeframe. 
Calls to the action are "debounced". The value is in milliseconds.

When the (optional) `debounceOnFolder` attribute is used, the folder-name is used for debouncing, instead of the filename.

The optional attribute `runOnStartup` can be used to run the associated action once, when the watcher is started. This can be helpful when
you have commands registered that run when a file is created in a folder, but when such a file already exists on start up.

##Valid event values
```
onCreated
onChanged
onRenamed
onDeleted
```