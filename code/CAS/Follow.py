'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import SubTask


class Follow(SubTask):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        SubTask.__init__(self)
        self.ID = 7
        self.attributes_list = ['until']
        self.validAttributes = {
            'until' : {
                'type':'action',
                'values':['Verify'],
                'usage':0.63,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def until(self, value=None):
        return self._attr('until', value)
