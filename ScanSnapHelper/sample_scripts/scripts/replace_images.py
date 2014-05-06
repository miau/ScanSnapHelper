import os.path
import shutil
import sys
import time

file = sys.argv[1]
basename = os.path.basename(file)
print "  replacing %s with ./%s" % (file, basename)
shutil.copy(basename, file)
