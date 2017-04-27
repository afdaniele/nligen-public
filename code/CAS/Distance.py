'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Description


class Distance(Description):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Description.__init__(self)
        self.ID = 3
        self.attributes_list = ['count', 'distUnit']
        self.validAttributes = {
            'count' : {
                'type':'enum',
                'values':[1, 2, 3, 4, 5, 6],
                'usage':0.88,
                'mandatory':True
            },
            'distUnit' : {
                'type':'action',
                'values':['Verify'],
                'usage':0.08,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def count(self, value=None):
        return self._attr('count', value)

    def distUnit(self, value=None):
        return self._attr('distUnit', value)
