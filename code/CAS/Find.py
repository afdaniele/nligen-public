'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Action


class Find(Action):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Action.__init__(self)
        self.ID = 6
        self.attributes_list = ['until']
        self.validAttributes = {
            'until' : {
                'type':'action',
                'values':['Verify'],
                'usage':1.00,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def until(self, value=None):
        return self._attr('until', value)
