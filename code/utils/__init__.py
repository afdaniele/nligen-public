'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 17, 2016 - Chicago - IL

'''

import copy
import time

try:
    import cPickle as pickle
except:
    import pickle


class Utils(object):
    '''
    classdocs
    '''

    @staticmethod
    def serialize(filename, obj):
        if filename[0] != '/': filename = './'+filename
        f = open(filename,'w')
        pickle.dump(obj,f)
        f.close()

    @staticmethod
    def deserialize(filename):
        f = open(filename,'r')
        res = pickle.load(f)
        f.close()
        return res

    @staticmethod
    def deep_copy( elem ):
        from CAS import StaticTree
        if isinstance( elem, list ):
            res = []
            for e in elem:
                res.append( Utils.deep_copy( e ) )
            return res
        elif isinstance( elem, tuple ):
            return tuple( Utils.deep_copy( list( elem ) ) )
        elif isinstance( elem, dict ):
            res = {}
            for k,v in elem.items():
                k1 = Utils.deep_copy( k )
                v1 = Utils.deep_copy( v )
                res[k1] = v1
            return res
        elif issubclass( elem.__class__, StaticTree.__class__ ):
            return elem.deep_copy()
        else:
            return copy.copy( elem )
