'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Action
from copy import copy


class Verify(Action):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Action.__init__(self)
        self.ID = 4
        self.attributes_list = ['desc', 'goal', 'dist']
        self.validAttributes = {
            'desc' : {
                'type':'list[N]', 'content-type':'description',
                'values':['Thing'],
                'usage':0.92,
                'mandatory':True
            },
            'goal' : {
                'type':'sub-task',
                'values':['DeclareGoal'],
                'usage':0.06,
                'mandatory':True
            },
            'dist' : {
                'type':'description',
                'values':['Distance'],
                'usage':0.01,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def desc(self, value=None):
        return self._attr('desc', value)

    def goal(self, value=None):
        return self._attr('goal', value)

    def dist(self, value=None):
        return self._attr('dist', value)
