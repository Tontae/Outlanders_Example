using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Character
{
    public enum Gender { Male, Female }

    public enum BodyParts
    {
        //male and female parts
        Face,
        Hair,
        Eyebrow,
        Beard,
        Torso,
        Arm_Upper_Right,
        Arm_Upper_Left,
        Arm_Lower_Right,
        Arm_Lower_Left,
        Hand_Right,
        Hand_Left,
        Hips,
        Leg_Right,
        Leg_Left
    }

    public enum BodySuits
    {
        //male and female parts
        HelmetAttachment,
        Helmet,
        Cuirass,
        ShoulderAttach_Left,
        ShoulderAttach_Right,
        Arm_Upper_Right,
        Arm_Upper_Left,
        Arm_Lower_Right,
        Arm_Lower_Left,
        Hand_Right,
        Hand_Left,
        Viel,
        Cuisses,
        KneeAttach_Left,
        KneeAttach_Right,
        Leg_Right,
        Leg_Left
    }

    public enum SuitParts
    {
        Main_Weapon,
        Sub_Weapon,
        Helm,
        Cuirass,
        Gauntlets,
        Viel,
        Cuisses,
        Greaves,
    }


    // classe for keeping the lists organized, allows for simple switching from male/female objects
    [System.Serializable]
    public class CharacterObjectGroups
    {
        public List<Mesh> face;
        public List<Mesh> hair;
        public List<Mesh> eyebrow;
        public List<Mesh> beard;
        public List<Mesh> torso;
        public List<Mesh> arm_Upper_Right;
        public List<Mesh> arm_Upper_Left;
        public List<Mesh> arm_Lower_Right;
        public List<Mesh> arm_Lower_Left;
        public List<Mesh> hand_Right;
        public List<Mesh> hand_Left;
        public List<Mesh> hips;
        public List<Mesh> leg_Right;
        public List<Mesh> leg_Left;
    }
}