'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 20, 2016 - Chicago - IL
'''

from Meanings import *
from utils import Utils
from StaticTree import StaticTree
from copy import deepcopy


class Tree(StaticTree):
    '''
    classdocs
    '''

    def __init__(self):
        '''
        Constructor
        '''
        self.ID = 0
        self.attributes = {}
        self.validAttributes = {}
        self.attributes_list = []


    def __populate__(self, args):
        for attr_name in args:
            self.attr(attr_name, args[attr_name])

    def __str__(self, omitMeanings=False):
        s = self.__class__.__name__+'('
        #
        printable_data = [ (k,self.attr(k)) for k in self.attributes_list if k in self.get_valid_attributes() ]
        for (key, val) in printable_data:
            s += key+'='
            if isinstance(val, list):
                s += '['+str(", ").join( [ 'None' if (omitMeanings and (isinstance(v,bool) or isinstance(v,int) or isinstance(v,str))) else ("'%s'" % str(v) if ( not omitMeanings and (isinstance(v,str)) ) else ( "%r" % v if (not omitMeanings and (isinstance(v,bool))) else ("%d" % v if (not omitMeanings and (isinstance(v,int))) else ( v.__str__(omitMeanings) ) ) ) ) for v in val] )+']'
            else:
                s += 'None' if (omitMeanings and (isinstance(val,bool) or isinstance(val,int) or isinstance(val,str))) else ("'%s'" % str(val) if ( not omitMeanings and (isinstance(val,str)) ) else ( "%r" % val if (not omitMeanings and (isinstance(val,bool))) else ("%d" % val if (not omitMeanings and (isinstance(val,int))) else ( val.__str__(omitMeanings) ) ) ) )
            s += ', '
        if len(printable_data) > 0:
            s = s[:-2]
        #
        s += ')'
        return s

    def deep_copy(self):
        node = StaticTree.from_string( self.__str__() )
        # perform deep copy
        node.__dict__ = Utils.deep_copy( self.__dict__ )
        #
        return node

    def attr_set(self, key, value):
        if key in self.validAttributes:
            # reject actions with zero arguments defined
            if self.validAttributes[key]['type'] in ['action', 'sub-task', 'description']:
                if value.get_valid_attributes_num() == 0: return
            elif self.validAttributes[key]['type'] in ['list[1]', 'list[N]'] and self.validAttributes[key]['content-type'] in ['action', 'sub-task', 'description']:
                tmp = []
                for v in value:
                    if v.get_valid_attributes_num() > 0:
                        tmp.append( v )
                value = tmp
                if len(value) == 0: return
            # set the attribute
            self.attributes[key] = value

    def attr_unset(self, key):
        self.attributes.pop( key, None )

    def attr_get(self, key, default=None):
        if key in self.attributes:
            return self.attributes[key]
        else:
            return default

    def get_valid_attributes(self):
        valid = []
        #
        for x in self.attributes:
            if self.validAttributes[x]['type'] == 'list[N]':
                if self.validAttributes[x]['content-type'] == 'enum':
                    valid.append( x )
                else:
                    if sum( [ v.get_valid_attributes_num() for v in self.attr(x) ] ) > 0:
                        valid.append( x )
            elif self.validAttributes[x]['type'] in ['action', 'sub-task', 'description']:
                if self.attr(x).get_valid_attributes_num() > 0:
                    valid.append( x )
            else:
                valid.append( x )
        return valid

    def get_valid_attributes_num(self):
        return len( self.get_valid_attributes() )

    def attr(self, name, value=None):
        return self._attr(name, value)

    def _attr(self, name, value=None):
        if name not in self.validAttributes:
            # Error
            raise ValueError(str(name)+' is not a valid attribute. It should fall in the set '+str(self.validAttributes.keys()))

        if value is None:
            # serve as getter
            val = self.attr_get(name)
            if val is None: return val
            #
            if( self.validAttributes[name]['type'] == 'list[1]' ):
                val = val[0]
            #
            return val
        else:
            # serve as setter
            if( self.validAttributes[name]['type'] == 'list[1]' ):
                if isinstance(value,list) and len(value) == 1:
                    # in this way I can use a single-value array as value
                    value = value[0]
                #
                if( self.validAttributes[name]['content-type'] == 'description' ):
                    if (not issubclass(value.__class__, Description)) or (value.__class__.__name__ not in self.validAttributes[name]['values']):
                        # Error
                        raise ValueError(str(value.__class__.__name__)+' is not a valid type. It should fall in the set '+str([k for k in self.validAttributes[name]['values']]))
                    else:
                        if self.attr_get(name) is not None:
                            print 'Warning in ['+self.__class__.__name__+']['+name+']: You\'re trying to convert a List[N] into a List[1]'
                        self.attr_set(name, [value])
                elif( self.validAttributes[name]['content-type'] == 'enum' ):
                    # handle the conversion between strings and integers
                    if isinstance( value, str ):
                        if value.isdigit():
                            value = int(value)
                    if value not in self.validAttributes[name]['values']:
                        # Error
                        raise ValueError('The object provided is not valid. It should fall in the set '+str([k.__repr__() for k in self.validAttributes[name]['values']] ) )
                    else:
                        self.attr_set(name, [value])
                elif( self.validAttributes[name]['content-type'] == 'integer' ):
                    val = value
                    if not isinstance(value, int):
                        val = None
                        # try to convert
                        if isinstance(value, str):
                            try:
                                val = int(value)
                            except ValueError:
                                val = None
                    if isinstance(val, int):
                        self.attr_set(name, [val])
                    else:
                        # Error
                        raise ValueError('The object provided is not valid. Only integers (and string representing integer values) are allowed.')
                elif( self.validAttributes[name]['content-type'] == 'text' ):
                    if not isinstance(value, str):
                        # Error
                        raise ValueError('The object provided is not valid. Only strings are allowed.')
                    else:
                        self.attr_set(name, [value])
                else:
                    raise ValueError('Type not valid')
            elif( self.validAttributes[name]['type'] == 'list[N]' ):
                lval = value
                if not isinstance(value, list):
                    # in this way I can treat the value as a list[N] object
                    lval = [value]
                #
                for value in lval:
                    if( self.validAttributes[name]['content-type'] == 'description' ):
                        if (not issubclass(value.__class__, Description)) or (value.__class__.__name__ not in self.validAttributes[name]['values']):
                            # Error
                            raise ValueError(str(value.__class__.__name__)+' is not a valid type. It should fall in the set '+str([k for k in self.validAttributes[name]['values']]))
                        else:
                            # add or append
                            lst = self.attr_get(name)
                            if lst is None:
                                self.attr_set(name, [value])
                            else:
                                lst.append(value)
                    elif( self.validAttributes[name]['content-type'] == 'enum' ):
                        # handle the conversion between strings and integers
                        if isinstance( value, str ):
                            if value.isdigit():
                                value = int(value)
                        if value not in self.validAttributes[name]['values']:
                            # Error
                            raise ValueError('The object provided is not valid. It should fall in the set '+str([k.__repr__() for k in self.validAttributes[name]['values']] ) )
                        else:
                            # add or append
                            lst = self.attr_get(name)
                            if lst is None:
                                self.attr_set(name, [value])
                            else:
                                lst.append(value)
                    else:
                        raise ValueError('Type not valid')
            elif( self.validAttributes[name]['type'] == 'action' ):
                if (not issubclass(value.__class__, Action)) or (value.__class__.__name__ not in self.validAttributes[name]['values']):
                    # Error
                    raise ValueError(str(value.__class__.__name__)+' is not a valid type. It should fall in the set '+str([k for k in self.validAttributes[name]['values']]))
                else:
                    self.attr_set(name, value)
            elif( self.validAttributes[name]['type'] == 'sub-task' ):
                if (not issubclass(value.__class__, SubTask)) or (value.__class__.__name__ not in self.validAttributes[name]['values']):
                    # Error
                    raise ValueError(str(value.__class__.__name__)+' is not a valid type. It should fall in the set '+str([k for k in self.validAttributes[name]['values']]))
                else:
                    self.attr_set(name, value)
            elif( self.validAttributes[name]['type'] == 'description' ):
                if (not issubclass(value.__class__, Description)) or (value.__class__.__name__ not in self.validAttributes[name]['values']):
                    # Error
                    raise ValueError(str(value.__class__.__name__)+' is not a valid type. It should fall in the set '+str([k for k in self.validAttributes[name]['values']]))
                else:
                    self.attr_set(name, value)
            elif( self.validAttributes[name]['type'] == 'enum' ):
                # handle the conversion between strings and integers
                if isinstance( value, str ):
                    if value.isdigit():
                        value = int(value)
                if value not in self.validAttributes[name]['values']:
                    if 'default' in self.validAttributes[name]:
                        self.attr_set(name, self.validAttributes[name]['default'])
                    else:
                        # Error
                        raise ValueError('The object provided is not valid. It should fall in the set '+str([k.__repr__() for k in self.validAttributes[name]['values']] ) )
                else:
                    self.attr_set(name, value)
            elif( self.validAttributes[name]['type'] == 'integer' ):
                val = value
                if not isinstance(value, int):
                    val = None
                    # try to convert
                    if isinstance(value, str):
                        try:
                            val = int(value)
                        except ValueError:
                            val = None
                if isinstance(val, int):
                    self.attr_set(name, val)
                else:
                    # Error
                    raise ValueError('The object provided is not valid. Only integers (and string representing integer values) are allowed.')
            elif( self.validAttributes[name]['type'] == 'text' ):
                if not isinstance(value, str):
                    # Error
                    raise ValueError('The object provided is not valid. Only strings are allowed.')
                else:
                    self.attr_set(name, value)
            else:
                raise ValueError('Type not valid')





class Action(Tree):
    '''
    classdocs
    '''


    def __init__(self):
        '''
        Constructor
        '''
        Tree.__init__(self)



class SubTask(Tree):
    '''
    classdocs
    '''


    def __init__(self):
        '''
        Constructor
        '''
        Tree.__init__(self)



class Description(Tree):
    '''
    classdocs
    '''


    def __init__(self):
        '''
        Constructor
        '''
        Tree.__init__(self)
