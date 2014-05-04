import os
import os.path
import sys

# JPEG Lossless Rotator must be installed ./jpegr_portable64
# http://annystudio.com/software/jpeglosslessrotator/
file = sys.argv[1]
basename = os.path.basename(file) # ScanSnap123.raw
num = int(basename[8:-4])
if num % 2 == 0:
    print "  rotating left %s" % (file)
    os.system('jpegr_portable64\jpegr.exe -l -s "%s"' % file)
else:
    print "  rotating right %s" % (file)
    os.system('jpegr_portable64\jpegr.exe -r -s "%s"' % file)
