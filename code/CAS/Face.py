'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Action
from Meanings import *


class Face(Action):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Action.__init__(self)
        self.ID = 5
        self.attributes_list = ['faced', 'direction']
        self.validAttributes = {
            'faced' : {
                'type':'action',
                'values':['Verify'],
                'usage':1.00,
                'mandatory':True
            },
            'direction' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[Left,Right,Front,Back],
                'usage':0.28,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def faced(self, value=None):
        return self._attr('faced', value)

    def direction(self, value=None):
        return self._attr('direction', value)
