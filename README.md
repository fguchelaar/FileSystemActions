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
</watchers>
```

##Valid event values
```
onCreated
onChanged
onRenamed
onDeleted
```