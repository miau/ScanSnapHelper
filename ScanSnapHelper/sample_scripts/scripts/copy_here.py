import os.path
import shutil
import sys

file = sys.argv[1]
basename = os.path.basename(file)
to = os.path.join('images', basename)
print "  copy %s -> %s" % (file, to)
shutil.copy(file, to)
