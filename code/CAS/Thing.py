'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Description
from Meanings import *


class Thing(Description):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Description.__init__(self)
        self.ID = 9
        self.attributes_list = ['type', 'value', 'dist', 'side', 'Appear', 'Part', 'Detail', 'Structural', 'Order_adj', 'On', 'Count', 'Past']
        self.validAttributes = {
            'type' : {
                'type':'enum',
                'values':[TypePath,TypeObj,TypeStruct,TypeRegion],
                'usage':1.00,
                'mandatory':True
            },
            'value' : {
                'type':'enum',
                'values':[Path,End,Wall,  Chair,GenChair,Hatrack,Lamp,Easel,Sofa,Barstool,Furniture,Empty,  Intersection,Corner,DeadEnd,Block,  Butterfly,Eiffel,Fish,Pic,  Rose,Wood,Grass,Cement,BlueTile,Brick,Stone,Honeycomb,Gray,Greenish,Brown,Dark,Flooring],
                'default':Path, # Used to correct 7 records with: value=[Back] in the original dataset
                'constrained-by':[ 'type' ], #TODO: not implemented
                'usage':1.00,
                'mandatory':True
            },
            'dist' : {
                'type':'enum',
                'values':[Immediate,Near,Far],
                'usage':0.82,
                'mandatory':True
            },
            'side' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[Front,At,Left,Right,Back,Sides],
                'usage':0.67,
                'mandatory':True
            },
            'Appear' : {
                'type':'list[N]', 'content-type':'enum',
                'values':[Rose,Wood,Grass,Cement,BlueTile,Brick,Stone,Honeycomb,Gray,Greenish,Brown,Dark,Flooring],
                'usage':0.33,
                'mandatory':True
            },
            'Part' : {
                'type':'list[N]', 'content-type':'description',
                'values':['Thing'],
                'usage':0.14,
                'mandatory':True
            },
            'Detail' : {
                'type':'list[N]', 'content-type':'description',
                'values':['Thing'],
                'usage':0.07,
                'mandatory':True
            },
            'Structural' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[T_Int,Long,Short],
                'usage':0.02,
                'mandatory':True
            },
            'Order_adj' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[1, 2, 3],
                'usage':0.02,
                'mandatory':True
            },
            'On' : {
                'type':'list[N]', 'content-type':'description',
                'values':['Thing'],
                'usage':0.02,
                'mandatory':True
            },
            'Count' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[1, 2, 3, 4],
                'usage':0.02,
                'mandatory':True
            },
            'Past' : {
                'type':'list[N]', 'content-type':'description',
                'values':['Thing'],
                'usage':0.01,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def type(self, value=None):
        return self._attr('type', value)

    def value(self, value=None):
        return self._attr('value', value)

    def dist(self, value=None):
        return self._attr('dist', value)

    def side(self, value=None):
        return self._attr('side', value)

    def appear(self, value=None):
        return self._attr('Appear', value)

    def part(self, value=None):
        return self._attr('Part', value)

    def detail(self, value=None):
        return self._attr('Detail', value)

    def structural(self, value=None):
        return self._attr('Structural', value)

    def order(self, value=None):
        return self._attr('Order_adj', value)

    def on(self, value=None):
        return self._attr('On', value)

    def count(self, value=None):
        return self._attr('Count', value)

    def past(self, value=None):
        return self._attr('Past', value)
