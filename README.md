FileSystemActions
=================

Provides a configurable Windows Service leveraging the FileSystemWatcher, to perform actions when a directory or file is created, changed, renamed or deleted.

#Configuration example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<watchers>
  <watcher path="c:\Temp">
    <action event="onCreated" command="onCreated.bat" />
    <action event="onChanged" command="onChanged.bat" />
  </watcher>
  <watcher path="c:\Temp">
    <action event="onChanged" command="onChangedAgain.bat" />
  </watcher>
  <watcher path="c:\Temp" filter="readme.*">
    <action event="onRenamed" command="onRenamed.bat" />
    <action event="onDeleted" command="onDeleted.bat" />
  </watcher>
  <watcher path="c:\Temp" filter="readme.*" timeout="5000">
    <action event="onRenamed" command="onRenamed.bat" />
    <action event="onDeleted" command="onDeleted.bat" />
  </watcher>
</watchers>
```

When using the (optional) `timeout` attribute, the actions will not get called multiple times if an event occurs within that timeframe. 
Calls to the action are "debounced". The value is in milliseconds.

##Valid event values
```
onCreated
onChanged
onRenamed
onDeleted
```