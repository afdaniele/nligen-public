'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Action
from Meanings import *


class Turn(Action):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Action.__init__(self)
        self.ID = 1
        self.attributes_list = ['direction', 'face', 'location', 'view', 'precond', 'postcond']
        self.validAttributes = {
            'direction' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[Left,Right,Front,Back],
                'usage':0.67,
                'mandatory':True
            },
            'face' : {
                'type':'action',
                'values':['Face'],
                'usage':0.32,
                'mandatory':True
            },
            'location' : {
                'type':'action',
                'values':['Travel'],
                'usage':0.14,
                'mandatory':True
            },
            'view' : {
                'type':'enum',
                'values':[True],
                'usage':0.06,
                'mandatory':True
            },
            'precond' : {
                'type':'action',
                'values':['Face', 'Travel'],
                'usage':0.04,
                'mandatory':True
            },
            'postcond' : {
                'type':'action',
                'values':['Travel'],
                'usage':0.01,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def direction(self, value=None):
        return self._attr('direction', value)

    def face(self, value=None):
        return self._attr('face', value)

    def location(self, value=None):
        return self._attr('location', value)

    def view(self, value=None):
        return self._attr('view', value)

    def precond(self, value=None):
        return self._attr('precond', value)

    def postcond(self, value=None):
        return self._attr('postcond', value)
