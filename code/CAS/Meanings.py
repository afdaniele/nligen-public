'''
Created by:

@author: Andrea F Daniele - TTIC - Toyota Technological Institute at Chicago
Feb 4, 2017 - Chicago - IL

'''

import re


class Meaning(object):

    mmap = {}

    @staticmethod
    def contains(abbreviation):
        if abbreviation in Meaning.mmap:
            return True
        else:
            return False

    @staticmethod
    def get(abbreviation):
        if Meaning.contains(abbreviation):
            return Meaning.mmap[abbreviation]
        else:
            return None

    @staticmethod
    def set(abbreviation, val):
        Meaning.mmap[abbreviation] = val

    @staticmethod
    def getPOS(wnword):
        wnword = Meaning.mean_string_to_wordnet_format( wnword )
        if re.match('(.+)_(.+)_(.+)', wnword) is not None:
            return wnword.split('_')[1]
        else:
            return wnword

    @staticmethod
    def mean_string_to_wordnet_format( mean_str ):
        if re.match('(.+)_(.+)_(.+)', mean_str) is not None:
            res = mean_str
            split_str = mean_str.split('_')
            last = split_str[2]
            tmp = None
            exec( 'tmp = %s' % last )
            if isinstance( tmp, tuple ):
                if isinstance( tmp[0], str ) and isinstance( tmp[1], list ):
                    res = '%s_%s_%d' % (tmp[0], split_str[1], tmp[1][0])
                elif isinstance( tmp[0], str ) and isinstance( tmp[1], int ):
                    res = '%s_%s_%d' % (tmp[0], split_str[1], tmp[1])
                elif isinstance( tmp[0], int ):
                    res = '%s_%s_%d' % (split_str[0], split_str[1], tmp[0])
                else:
                    print 'ERROR1: (%s) :: %s' % (mean_str, str(type(tmp[1])))  #TODO
            elif isinstance( tmp, int ):
                res = '%s_%s_%d' % (split_str[0], split_str[1], tmp)
            else:
                print 'ERROR2: (%s)' % mean_str  #TODO
            return res
        else:
            return mean_str

    def __init__(self,n,a,m):
        self.name = n
        self.abbreviation = a
        self.meaning = m
        Meaning.mmap[a] = self

    def __str__(self, omitMeanings=False): return self.name if not omitMeanings else 'None'
    def __repr__(self): return self.abbreviation
    def equals(self,other): return isinstance(other, self.__class__) and self.__repr__() == other.__repr__()
    def __text__(self): return self.meaning.split('_')[0]
    def __eq__(self, other): return isinstance(other, self.__class__) and self.__repr__() == other.__repr__()
    def __ne__(self, other): return not self.__eq__(other)
    def __hash__(self): return id(self)


TypeObj = Meaning('TypeObj', 'Obj', 'Object')
TypePath = Meaning('TypePath', 'Path', 'Path')
TypeStruct = Meaning('TypeStruct', 'Struct', 'Structure')
TypeRegion = Meaning('TypeRegion', 'Region', 'Region')

Types = {
    'TypeObj' : TypeObj,
    'TypePath' : TypePath,
    'TypeStruct' : TypeStruct,
    'TypeRegion' : TypeRegion
}


class Side(Meaning):
    def __init__(self,n,a,m):
        Meaning.__init__(self,n,a,m)

Left = Side('Left','<','Left')
Right = Side('Right','>','Right')
Back = Side('Back','[','Back')
Front = Side('Front',']','Front')
At = Side('At','@', 'At')
Sides = Side('Sides','=','Sides')

Opposites = {
    Left : Right,
    Right : Left,
    Back : Front,
    Front : Back,
    At : At,
    Sides : Sides
}
def opposite(side): return Opposites[side]

Directions = {
    'RT' : Right,
    'right_ADV_4' : Right,
    'right_N_3' : Right,
    'LFT' : Left,
    'LT' : Left,
    'left_ADV_1' : Left,
    'left_V_1' : Left,
    'BACK' : Back,
    'BEHIND' : Back,
    'around_ADV_6' : Back,
    'back_ADV_1' : Back,
    'back_N_1' : Back,
    'behind_ADV_1' : Back,
    'AHEAD' : Front,
    'DOWN' : Front,
    'FOWARD' : Front,
    'FWD' : Front,
    'IN' : Front,
    'OUT' : Front,
    'UP' : Front,
    'forward_ADV_1' : Front,
    'front_ADJ_1' : Front,
    'front_ADV_1' : Front,
    'front_N_2' : Front,
    'straight_ADJ_2' : Front,
    'straight_ADV_1' : Front,
    'side_N_1' : Sides,
    'side_ADJ_1' : Sides,
}


Counts = {
    '1_ADJ_1' : 1,
    '1_N_1' : 1,
    '1st_ADJ_1' : 1,
    'first_ADJ_1' : 1,
    'next_ADJ_3' : 1,
    'once_ADJ_1' : 1,
    'once_ADV_1' : 1,
    'one_ADJ_1' : 1,
    'one_N_1' : 1,
    '2_ADJ_1' : 2,
    '2_N_1' : 2,
    'second_ADJ_1' : 2,
    'twice_ADJ_1' : 2,
    'twice_ADV_1' : 2,
    'two_ADJ_1' : 2,
    'two_N_1' : 2,
    '3_ADJ_1' : 3,
    '3_N_1' : 3,
    'third_ADJ_1' : 3,
    'three_ADJ_1' : 3,
    'three_N_1' : 3,
    '4_ADJ_1' : 4,
    '4_N_1' : 4,
    'four_ADJ_1' : 4,
    'four_N_1' : 4,
    'fourth_ADJ_1' : 4,
    '5_ADJ_1' : 5,
    '5_N_1' : 5,
    'five_ADJ_1' : 5,
    'five_N_1' : 5,
    '6_ADJ_1' : 6,
    '6_N_1' : 6,
    'six_ADJ_1' : 6,
    'six_N_1' : 6,
    '7_ADJ_1' : 7,
    '7_N_1' : 7,
    'seven_ADJ_1' : 7,
    'seven_N_1' : 7,
    '8_ADJ_1' : 8,
    '8_N_1' : 8,
    'eight_ADJ_1' : 8,
    'eight_N_1' : 8,
    '9_ADJ_1' : 9,
    '9_N_1' : 9,
    'nine_ADJ_1' : 9,
    'nine_N_1' : 9,
    'few_ADJ_1' : 3,
    'last_ADJ_2' : -1,
    'ONE' : 1,
    'TWO' : 2,
    'THREE' : 3,
    'FOUR' : 4,
    'FIVE' : 5,
    'SIX' : 6,
    'SEVEN' : 7,
    'EIGHT' : 8,
    'NINE' : 9,
    '1' : 1,
    '2' : 2,
    '3' : 3,
    '4' : 4,
    '5' : 5,
    '6' : 6,
    '7' : 7,
    '8' : 8,
    '9' : 9
}



class Texture(Meaning):
    def __init__(self,n,a,m): Meaning.__init__(self,n,a,m)

Rose = Texture('Rose', 'r', 'rose_ADJ_1')
Wood = Texture('Wood', 'w', 'wooden_ADJ_1')
Grass = Texture('Grass', 'g', 'grassy_ADJ_1')
Cement = Texture('Cement', 'c', 'cement_N_1')
BlueTile = Texture('BlueTile', 't', 'blue_ADJ_1')
Brick = Texture('Brick', 'b', 'brick_N_1')
Stone = Texture('Stone','s', 'stone_ADJ_1')
Honeycomb = Texture('Honeycomb', 'h', 'honeycomb_N_1')
Flooring = Texture('Flooring', 'cbghsrtw', 'flooring_N_2')
Gray = Texture('Gray', 'cs', 'gray_ADJ_1')
Greenish = Texture('Greenish', 'gh', 'green_ADJ_1')
Brown = Texture('Brown', 'bw', 'brown_ADJ_1')
Dark = Texture('Dark', 'stwb', 'dark_ADJ_2')

Textures = {
    # used while loading the map from the XML file
    'flower' : Rose,
    'wood' : Wood,
    'grass' : Grass,
    'concrete' : Cement,
    'blue' : BlueTile,
    'brick' : Brick,
    'gravel' : Stone,
    'yellow' : Honeycomb,
    # used with the POS alignment
    'flower_N_1' : Rose,
    'flower_N_2' : Rose,
    'flowered_ADJ_1' : Rose,
    'pink_ADJ_1' : Rose,
    'pink_N_1' : Rose,
    'rose_ADJ_1' : Rose,
    'orange_ADJ_1' : Wood,
    'wood_ADJ_1' : Wood,
    'wood_N_1' : Wood,
    'wooden_ADJ_1' : Wood,
    'wooden_N_1' : Wood,
    'brown_ADJ_1' : Brown,
    'green_ADJ_1' : Greenish,
    'grass_ADJ_1' : Grass,
    'grass_N_1' : Grass,
    'grassy_ADJ_1' : Grass,
    'bare_ADJ_4' : Cement,
    'cement_ADJ_1' : Cement,
    'cement_N_1' : Cement,
    'concrete_ADJ_2' : Cement,
    'plain_ADJ_3' : Cement,
    'white_ADJ_4' : Cement,
    'gray_ADJ_1' : Gray,
    'grey_N_1' : Gray,
    'blue_ADJ_1' : BlueTile,
    'brick_ADJ_1' : Brick,
    'brick_N_1' : Brick,
    'red_ADJ_1' : Brick,
    'black_ADJ_1' : Stone,
    'black_N_1' : Stone,
    'rock_ADJ_2' : Stone,
    'stone_ADJ_1' : Stone,
    'stone_N_2' : Stone,
    'hexagon_N_1' : Honeycomb,
    'hexagonal_ADJ_1' : Honeycomb,
    'honeycomb_N_1' : Honeycomb,
    'octagon_ADJ_1' : Honeycomb,
    'octagon_N_1' : Honeycomb,
    'olive_ADJ_1' : Honeycomb,
    'yellow_ADJ_1' : Honeycomb,
    'carpet_ADJ_1' : Flooring,
    'carpet_N_1' : Flooring,
    'carpeted_ADJ_1' : Flooring,
    'floor_N_1' : Flooring,
    'floored_ADJ_1' : Flooring,
    'flooring_N_1' : Flooring,
    'tile_N_1' : Flooring,
    'tiled_ADJ_1' : Flooring,
    'dark_ADJ_2' : Dark,
}



class Picture(Meaning):
    def __init__(self,n,a,m): Meaning.__init__(self,n,a,m)

Butterfly = Picture('Butterfly', '8', 'butterfly_N_1')
Eiffel = Picture('Eiffel', '7', 'eiffel_N_1')
Fish = Picture('Fish', '6', 'fish_N_1')
Pic = Picture('Pic', '876', 'picture_N_2')

Pictures = {
    # used while loading the map from the XML file
    'butterfly' : Butterfly,
    'tower' : Eiffel,
    'fish' : Fish,
    # used with the POS alignment
    'butterfly_ADJ_1' : Butterfly,
    'butterfly_N_1' : Butterfly,
    'eiffel_ADJ_1' : Eiffel,
    'eiffel_N_1' : Eiffel,
    'tower_ADJ_1' : Eiffel,
    'tower_N_1' : Eiffel,
    'eiffel tower_N_1' : Eiffel,
    'fish_ADJ_1' : Fish,
    'fish_N_1' : Fish,
    'pic_N_2' : Pic,
    'picture_N_2' : Pic,
    'hanging_N_1' : Pic,
}



class Object(Meaning):
    def __init__(self,n,a,m): Meaning.__init__(self,n,a,m)

Chair = Object('Chair', 'C', 'chair_N_1')
Sofa = Object('Sofa', 'S', 'sofa_N_1')
Barstool = Object('Barstool', 'B', 'stool_N_1')
Hatrack = Object('Hatrack', 'H', 'hatrack_N_1')
Easel = Object('Easel', 'E', 'easel_N_1')
Lamp = Object('Lamp', 'L', 'lamp_N_1')
Furniture = Object('Furniture', 'CBEHLS', 'furniture_N_1')
Seat = Object('Seat', 'CBS', 'seat_N_3')
GenChair = Object('GenChair', 'CS', 'chair_N_1')
Empty = Object('Empty', 'O', 'empty_ADJ_1')

Objects = {
    # used while loading the map from the XML file
    'chair' : Chair,
    'hatrack' : Hatrack,
    'lamp' : Lamp,
    'easel' : Easel,
    'sofa' : Sofa,
    'barstool' : Barstool,
    # used with the POS alignment
    'straight chair_N_1' : Chair,
    'chair_N_1' : GenChair,
    'stool_N_1' : Barstool,
    'bench_N_1' : Sofa,
    'sofa_N_1' : Sofa,
    'hatrack_N_1' : Hatrack,
    'coatrack_N_1' : Hatrack,
    'coat_N_1' : Hatrack,
    'rack_N_1' : Hatrack,
    'easel_N_1' : Easel,
    'lamp_N_1' : Lamp,
    'lamp_N_2' : Lamp,
    'pole_N_1' : Lamp,
    'furniture_N_1' : Furniture,
    'object_N_1' : Furniture,
    'empty_ADJ_1' : Empty,
    'vacant_ADJ_2' : Empty,
    'nothing_N_1' : Empty,
}



class Structure(Meaning):
    def __init__(self,n,a,m): Meaning.__init__(self,n,a,m)



class LinearStructure(Structure):
    def __init__(self,n,a,m): Structure.__init__(self,n,a,m)

Path = LinearStructure('Path', 'cbghsrtw\|', 'path_N_2')
End = LinearStructure('End', 'x', 'end_N_1')
Wall = LinearStructure('Wall', 'q', 'wall_N_1')



class AreaStructure(Structure):
    def __init__(self,n,a,m): Structure.__init__(self,n,a,m)

Intersection = AreaStructure('Intersection', '\+', 'intersection_N_2')
Corner = AreaStructure('Corner', '^', 'corner_N_4')
DeadEnd = AreaStructure('DeadEnd', '\!', 'dead end_N_1')
Block = AreaStructure('Block', '\:', 'block_N_2')
T_Int = AreaStructure('T_Int','T','t_N_5')

Structures = {
    'back_ADV_1' : Back,
    'block_N_2' : Block,
    'room_N_1' : Block,
    'corner_N_4' : Corner,
    'l_N_5' : Corner,
    'dead end_N_1' : DeadEnd,
    'end_N_1' : End,
    'end_V_1' : End,
    'branch_V_3' : Intersection,
    'cross_V_1' : Intersection,
    'cross_N_4' : Intersection,
    'intersect_V_1' : Intersection,
    'intersection_N_2' : Intersection,
    'open_V_8' : Intersection,
    'opening_N_1' : Intersection,
    'meet_V_3' : Intersection,
    'hallway_N_1' : Path,
    'alley_N_1' : Path,
    'carpet_N_1' : Path,
    'corridor_N_1' : Path,
    'exit_N_1' : Path,
    'floor_N_1' : Path,
    'path_N_3' : Path,
    'section_N_4' : Path,
    'hall_N_1' : Path,
    'segment_N_1' : Path,
    't_N_5' : T_Int,
    'wall_N_1' : Wall
}

Short = Meaning('Short', 'Short', 'short_ADJ_2')
Long = Meaning('Long', 'Long', 'long_ADJ_2')
Structurals = {
    'short_ADJ_2' : Short,
    'shorter_ADJ_1' : Short,
    'long_ADJ_2' : Long,
    'longer_ADJ_1' : Long
}

Immediate = Meaning('Immediate', '0', 'immmediate_ADJ_1')
Near = Meaning('Near', '0:', 'near_ADJ_1')
Far = Meaning('Far', '1:', 'far_ADJ_1')
Reldists = {
    'immmediate_ADJ_1' : Immediate,
    'near_ADJ_1' : Near,
    'far_ADJ_1' : Far,
    'other_ADJ_1' : Far,
    'opposite_ADJ_1' : Far,
    'farther_ADJ_1' : Far,
    'farthest_ADJ_1' : Far,
}

Defaults = {
    'Obj' : Furniture,
    'Path' : Path,
    'Struct' : Intersection,
    'Region' : Path,
    'Boolean' : True,
    'AreaStructure' : Block,
    'LinearStructure' : Path,
    'Object' : Furniture,
    'Picture' : Pic,
    'Texture' : Flooring,
    'Side' : Front
}


# add the removed (Linear Structure) meanings as default values
Meaning.set('!', Path) # Segment
Meaning.set('\^', Path) # PathDir

# add the removed (Area Structure) meanings as default values
Meaning.set('\.', Path) # Position
#KnowledgeMap.set('^', Path) # Corner
Meaning.set('m', Path) # Middle
Meaning.set('p', Path) # TopoPlace

# add the removed (Thing Type) meanings as default values
Meaning.set('Pathdir', TypePath) # Position
Meaning.set('Position', TypePath) # Corner
Meaning.set('Side', TypePath) # Middle
Meaning.set('Thing', TypePath) # TopoPlace
