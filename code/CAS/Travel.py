'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from CAS import Action


class Travel(Action):
    '''
    classdocs
    '''

    def __init__(self, **args):
        '''
        Constructor
        '''
        Action.__init__(self)
        self.ID = 2
        self.attributes_list = ['until', 'distance', 'face', 'past', 'follow', 'location']
        self.validAttributes = {
            'until' : {
                'type':'action',
                'values':['Verify'],
                'usage':0.62,
                'mandatory':True
            },
            'distance' : {
                'type':'list[1]', 'content-type':'description',
                'values':['Distance'],
                'usage':0.42,
                'mandatory':True
            },
            'face' : {
                'type':'action',
                'values':['Face'],
                'usage':0.24,
                'mandatory':True
            },
            'past' : {
                'type':'action',
                'values':['Verify'],
                'usage':0.05,
                'mandatory':True
            },
            'follow' : {
                'type':'sub-task',
                'values':['Follow'],
                'usage':0.04,
                'mandatory':True
            },
            'location' : {
                'type':'action',
                'values':['Travel'],
                'usage':0.01,
                'mandatory':True
            }
        }
        self.__populate__(args)


    def until(self, value=None):
        return self._attr('until', value)

    def dist(self, value=None): #TODO
        return self._attr('distance', value)

    def face(self, value=None):
        return self._attr('face', value)

    def past(self, value=None):
        return self._attr('past', value)

    def follow(self, value=None):
        return self._attr('follow', value)

    def location(self, value=None):
        return self._attr('location', value)
