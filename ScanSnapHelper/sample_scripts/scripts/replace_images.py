import os.path
import shutil
import sys
import time

file = sys.argv[1]
basename = os.path.basename(file)
frompath = os.path.join('images', basename)
print "  replacing %s with %s" % (file, frompath)
shutil.copy(frompath, file)
