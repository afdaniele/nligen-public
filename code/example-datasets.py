'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 5, 2017 - Chicago - IL
'''

# import
from CAS import StaticTree
from utils import Utils
import sys


# load datasets
print 'Loading ORIGINAL dataset...',; sys.stdout.flush()
odata = Utils.deserialize('../data/SAIL-original.pickle')
print 'Done!'
#
print 'Loading AUGMENTED dataset...',; sys.stdout.flush()
adata = Utils.deserialize('../data/SAIL-augmented.pickle')
print 'Done!\n'

# print some statistics
print 'ORIGINAL dataset:'
print '\tDemonstrations: %d' % len(odata)
#
print 'AUGMENTED dataset:'
print '\tDemonstrations: %d\n' % sum([ sum([ len(adata[ex_id]['segments'][seg_id]) for seg_id in adata[ex_id]['segments'] ]) for ex_id in adata ])

# get demonstration from original dataset
ex_id = 2
dem_map = odata[ex_id]['map']
dem_segments_num = len(odata[ex_id]['segments'])
#
print 'Example of demonstration from the ORIGINAL dataset:'
if ex_id in odata:
    print 'Demonstration (ex_id = %d);' % ex_id
    print 'Total segments: %d;' % dem_segments_num
    for seg_id in range(1,dem_segments_num+1):
        print '\tSegment %d:' % seg_id
        print '\t\tPath: %r' % odata[ex_id]['segments'][seg_id]['path']
        print '\t\tInstruction: %r' % odata[ex_id]['segments'][seg_id]['english']
        print '\t\tCAS Command: %r' % odata[ex_id]['segments'][seg_id]['cas']
print ''

# get demonstration from augmented dataset
ex_id = 2
seg_id = 1
#
print 'Example of demonstrations from the AUGMENTED dataset:'
if ex_id in adata:
    print '%d total demonstrations generated from (ex_id = %d, seg_id = %d);' % (len(adata[ex_id]['segments'][seg_id]), ex_id, seg_id)
    for dem in adata[ex_id]['segments'][seg_id]:
        print '\tInstruction: %r' % dem['english']
        print '\tCAS Command: %r' % dem['cas']
        print '\t----'
print ''


# parse CAS command and navigate it
print 'Example of CAS command as a Python object:'
ex_id = 2
seg_id = 1
C_str = odata[ex_id]['segments'][seg_id]['cas'][0]      # C_str is a string
C = StaticTree.from_string( C_str )                     # C is a Python object
print 'Demonstration (ex_id = %d, seg_id = %d);' % (ex_id, seg_id)
print '\tCAS Command: %s' % C_str
print '\tC = %s' % C
print '\tC.faced = %s' % C.faced()
print '\tC.faced.desc[0] = %s' % C.faced().desc()[0]
print '\tC.faced.desc[0].value = %s\n' % C.faced().desc()[0].value()


# print CAS command / CAS structure
print 'Example of CAS command / CAS structure:'
ex_id = 2
seg_id = 1
C_str = odata[ex_id]['segments'][seg_id]['cas'][0]
C = StaticTree.from_string( C_str )
print 'Demonstration (ex_id = %d, seg_id = %d);' % (ex_id, seg_id)
print '\tCAS Command: %s' % C.__str__()
print '\tCAS Structure: %s' % C.__str__(omitMeanings=True)
