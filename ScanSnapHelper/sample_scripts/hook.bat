cd "%~dp0"
echo %date%%time% *** hooked %* ***>> hook.log
python scripts\copy_here.py %*>> hook.log 2>&1
::python scripts\replace_images.py %*>> hook.log 2>&1
::python scripts\rotate_for_comic.py %*>> hook.log 2>&1
