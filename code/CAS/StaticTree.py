'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Apr 3, 2016 - Chicago - IL
'''

class StaticTree(object):
    '''
    classdocs
    '''
    
    @staticmethod
    def from_string(action_str):
        # load cas structure
        from CAS import Meaning
        from CAS import Tree
        from CAS.Turn import Turn
        from CAS.Travel import Travel
        from CAS.Distance import Distance
        from CAS.Verify import Verify
        from CAS.Face import Face
        from CAS.Find import Find
        from CAS.Follow import Follow
        from CAS.DeclareGoal import DeclareGoal
        from CAS.Thing import Thing
        from CAS.Meanings import *
        #
        if isinstance( action_str, list ):
            action_str = action_str.__repr__()
        #
        obj = eval( action_str )
        #
        if isinstance( obj, list ):
            tmp = []
            for e in obj:
                tmp.append( eval(e) )
            obj = tmp
        #
        return obj
