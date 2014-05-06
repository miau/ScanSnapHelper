cd "%~dp0"
echo %date%%time% *** hooked %* ***>> hook.log
python copy_here.py %*>> hook.log 2>&1
::python replace_images.py %*>> hook.log 2>&1
::python rotate_for_comic.py %*>> hook.log 2>&1
