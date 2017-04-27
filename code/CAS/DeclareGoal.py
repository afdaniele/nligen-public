'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import SubTask


class DeclareGoal(SubTask):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        SubTask.__init__(self)
        self.ID = 8
        self.attributes_list = ['cond', 'goal']
        self.validAttributes = {
            'cond' : {
                'type':'action',
                'values':['Travel'],
                'usage':0.94,
                'mandatory':True
            },
            'goal' : {
                'type':'list[1]', 'content-type':'enum',
                'values':[1, 2, 3, 4, 5, 6, 7],
                'usage':0.88,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def cond(self, value=None):
        return self._attr('cond', value)

    def goal(self, value=None):
        return self._attr('goal', value)
