import os.path
import shutil
import sys

file = sys.argv[1]
basename = os.path.basename(file)
print "  copy %s -> %s" % (file, basename)
shutil.copy(file, basename)
