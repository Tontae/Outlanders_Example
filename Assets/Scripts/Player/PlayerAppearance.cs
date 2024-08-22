using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Outlander.Character;
using PsychoticLab;
using static Cinemachine.CinemachinePathBase;

public class PlayerAppearance : PlayerElements
{
    private SkinnedMeshRenderer face;
    private SkinnedMeshRenderer hair;
    private SkinnedMeshRenderer eyebrow;
    private SkinnedMeshRenderer beard;
    private SkinnedMeshRenderer torso;
    private SkinnedMeshRenderer arm_Upper_Left;
    private SkinnedMeshRenderer arm_Upper_Right;
    private SkinnedMeshRenderer hand_Left;
    private SkinnedMeshRenderer hand_Right;
    private SkinnedMeshRenderer arm_Lower_Left;
    private SkinnedMeshRenderer arm_Lower_Right;
    private SkinnedMeshRenderer hips;
    private SkinnedMeshRenderer leg_Right;
    private SkinnedMeshRenderer leg_Left;

    public SkinnedMeshRenderer Face
    {
        get { return face; }
        set
        {
            if (face != value)
            {
                face = value;
                face_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Hair
    {
        get { return hair; }
        set
        {
            if (hair != value)
            {
                hair = value;
                hair_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Eyebrow
    {
        get { return eyebrow; }
        set
        {
            if (eyebrow != value)
            {
                eyebrow = value;
                eyebrow_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Beard
    {
        get { return beard; }
        set
        {
            if (beard != value)
            {
                beard = value;
                beard_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Torso
    {
        get { return torso; }
        set
        {
            if (torso != value)
            {
                torso = value;
                torso_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Upper_Left
    {
        get { return arm_Upper_Left; }
        set
        {
            if (arm_Upper_Left != value)
            {
                arm_Upper_Left = value;
                arm_Upper_Left_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Upper_Right
    {
        get { return arm_Upper_Right; }
        set
        {
            if (arm_Upper_Right != value)
            {
                arm_Upper_Right = value;
                arm_Upper_Right_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Lower_Left
    {
        get { return arm_Lower_Left; }
        set
        {
            if (arm_Lower_Left != value)
            {
                arm_Lower_Left = value;
                arm_Lower_Left_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Arm_Lower_Right
    {
        get { return arm_Lower_Right; }
        set
        {
            if (arm_Lower_Right != value)
            {
                arm_Lower_Right = value;
                arm_Lower_Right_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Hand_Left
    {
        get { return hand_Left; }
        set
        {
            if (hand_Left != value)
            {
                hand_Left = value;
                hand_Left_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Hand_Right
    {
        get { return hand_Right; }
        set
        {
            if (hand_Right != value)
            {
                hand_Right = value;
                hand_Right_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Hips
    {
        get { return hips; }
        set
        {
            if (hips != value)
            {
                hips = value;
                hips_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Leg_Left
    {
        get { return leg_Left; }
        set
        {
            if (leg_Left != value)
            {
                leg_Left = value;
                leg_Left_Mesh = value.sharedMesh;
            }
        }
    }
    public SkinnedMeshRenderer Leg_Right
    {
        get { return leg_Right; }
        set
        {
            if (leg_Right != value)
            {
                leg_Right = value;
                leg_Right_Mesh = value.sharedMesh;
            }
        }
    }

    SkinnedMeshRenderer[] rootTransform;

    public AppearanceScriptable appearanceScriptable;
    public Material mat;

    public Mesh face_Mesh = null;
    public Mesh hair_Mesh = null;
    public Mesh eyebrow_Mesh = null;
    public Mesh beard_Mesh = null;
    public Mesh torso_Mesh = null;
    public Mesh arm_Upper_Left_Mesh = null;
    public Mesh arm_Upper_Right_Mesh = null;
    public Mesh hand_Left_Mesh = null;
    public Mesh hand_Right_Mesh = null;
    public Mesh arm_Lower_Left_Mesh = null;
    public Mesh arm_Lower_Right_Mesh = null;
    public Mesh hips_Mesh = null;
    public Mesh leg_Right_Mesh = null;
    public Mesh leg_Left_Mesh = null;

    private void Start()
    {
        if(isClient && !isLocalPlayer)
        {
            if (rootTransform == null)
                CommandSetApearance();
        }
    }

    private void InitAppearance()
    {
        if (isServer) return;
        rootTransform = transform.GetChild(1).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        Face = FindAppearancePart("Chr_Face");
        Hair = FindAppearancePart("Chr_Hair");
        Eyebrow = FindAppearancePart("Chr_Eyebrow");
        Beard = FindAppearancePart("Chr_Beard");
        Torso = FindAppearancePart("Chr_Torso");
        Arm_Upper_Left = FindAppearancePart("Chr_ArmUpperLeft");
        Arm_Upper_Right = FindAppearancePart("Chr_ArmUpperRight");
        Arm_Lower_Left = FindAppearancePart("Chr_ArmLowerLeft");
        Arm_Lower_Right = FindAppearancePart("Chr_ArmLowerRight");
        Hand_Left = FindAppearancePart("Chr_HandLeft");
        Hand_Right = FindAppearancePart("Chr_HandRight");
        Hips = FindAppearancePart("Chr_Hips");
        Leg_Right = FindAppearancePart("Chr_LegLeft");
        Leg_Left = FindAppearancePart("Chr_LegRight");

        mat = Face.material;
        Face.material = mat;
        Hair.material = mat;
        Eyebrow.material = mat;
        Beard.material = mat;
        Torso.material = mat;
        Arm_Upper_Left.material = mat;
        Arm_Upper_Right.material = mat;
        Arm_Lower_Left.material = mat;
        Arm_Lower_Right.material = mat;
        Hand_Left.material = mat;
        Hand_Right.material = mat;
        Hips.material = mat;
        Leg_Left.material = mat;
        Leg_Right.material = mat;

        //SetAppearance();
    }

    SkinnedMeshRenderer FindAppearancePart(string _name)
    {
        foreach (SkinnedMeshRenderer t in rootTransform)
        {
            if (t.gameObject.name == _name)
            {
                return t;
            }
        }
        return null;
    }

    public void SetAppearance(CharacterAppearance[] appearances, int gender)
    {
        if(appearances != null)
        {
            //for (int i = 0; i < appearances.Length; i++)
                //Debug.Log($"START OF SETAPPEARNCE {appearances[i].id} & {appearances[i].color}");

            if (gender == 0 || gender == 2)
            {
                foreach (Mesh t in appearanceScriptable.maleFace)
                {
                    if (CheckAppearance(t.name, appearances[(int)BodyParts.Face].id))
                    {
                        Face.sharedMesh = t;
                        face_Mesh = t;
                    }
                }

                foreach (Mesh t in appearanceScriptable.maleEyebrow)
                {
                    if (CheckAppearance(t.name, appearances[(int)BodyParts.Eyebrow].id))
                    {
                        Eyebrow.sharedMesh = t;
                        eyebrow_Mesh = t;
                    }
                }
            }
            else
            {
                foreach (Mesh t in appearanceScriptable.femaleFace)
                {
                    if (CheckAppearance(t.name, appearances[(int)BodyParts.Face].id))
                    {
                        Face.sharedMesh = t;
                        face_Mesh = t;
                    }
                }

                foreach (Mesh t in appearanceScriptable.femaleEyebrow)
                {
                    if (CheckAppearance(t.name, appearances[(int)BodyParts.Eyebrow].id))
                    {
                        Eyebrow.sharedMesh = t;
                        eyebrow_Mesh = t;
                    }
                }
            }
            foreach (Mesh t in appearanceScriptable.hair)
            {
                if (CheckAppearance(t.name, appearances[(int)BodyParts.Hair].id))
                {
                    Hair.sharedMesh = t;
                    hair_Mesh = t;
                }
            }
            foreach (Mesh t in appearanceScriptable.beard)
            {
                if (CheckAppearance(t.name, appearances[(int)BodyParts.Beard].id))
                {
                    Beard.sharedMesh = t;
                    beard_Mesh = t;
                }
            }
            Color _color = appearances[(int)BodyParts.Face].color.ToColor();

            mat.SetColor("_Color_Skin_1", _color);
            float currentHue = 0f;
            float currentSat = 0f;
            float currentVal = 0f;
            Color.RGBToHSV(_color, out currentHue, out currentSat, out currentVal);
            currentVal -= 0.1f;
            mat.SetColor("_Color_Beard_2", Color.HSVToRGB(currentHue, currentSat, currentVal));
            currentVal -= 0.2f;
            mat.SetColor("_Color_Skin_2", Color.HSVToRGB(currentHue, currentSat, currentVal));
            currentVal -= 0.2f;
            mat.SetColor("_Color_Tattoo", Color.HSVToRGB(currentHue, currentSat, currentVal));

            mat.SetColor("_Color_Hair", appearances[(int)BodyParts.Hair].color.ToColor());
            mat.SetColor("_Color_Eyebrow", appearances[(int)BodyParts.Eyebrow].color.ToColor());
            mat.SetColor("_Color_Eye", Color.black);
            mat.SetColor("_Color_Beard_1", appearances[(int)BodyParts.Beard].color.ToColor());


            //for (int i = 0; i < appearances.Length; i++)
            //Debug.Log($"END OF SETAPPEARNCE {appearances[i].id} & {appearances[i].color}");
        }
    }

    private bool CheckAppearance(string name,string playerId)
    {
        if (name.Split("_")[name.Split("_").Length - 1] == playerId.Split("_")[playerId.Split("_").Length - 1])
            return true;
        else
            return false;
    }

    public void OnCloseHead(bool _close)
    {
        if (_close)
        {
            Face.sharedMesh = null;
            Hair.sharedMesh = null;
            Eyebrow.sharedMesh = null;
            Beard.sharedMesh = null;
        }
        else
        {
            //if (Face == null) return;
            Face.sharedMesh = face_Mesh;
            Hair.sharedMesh = hair_Mesh;
            Eyebrow.sharedMesh = eyebrow_Mesh;
            Beard.sharedMesh = beard_Mesh;
        }
    }

    public void OnCloseCuirass(bool _close)
    {
        if (_close)
        {
            Torso.sharedMesh = null;
            Arm_Upper_Left.sharedMesh = null;
            Arm_Upper_Right.sharedMesh = null;
        }
        else
        {
            //if (Torso == null) return;
            Torso.sharedMesh = torso_Mesh;
            Arm_Upper_Left.sharedMesh = arm_Upper_Left_Mesh;
            Arm_Upper_Right.sharedMesh = arm_Upper_Right_Mesh;
        }
    }

    public void OnCloseGauntlets(bool _close)
    {
        if (_close)
        {
            Arm_Lower_Left.sharedMesh = null;
            Arm_Lower_Right.sharedMesh = null;
            Hand_Left.sharedMesh = null;
            Hand_Right.sharedMesh = null;
        }
        else
        {
            //if (Arm_Lower_Left == null) return;
            Arm_Lower_Left.sharedMesh = arm_Lower_Left_Mesh;
            Arm_Lower_Right.sharedMesh = arm_Lower_Right_Mesh;
            Hand_Left.sharedMesh = hand_Left_Mesh;
            Hand_Right.sharedMesh = hand_Right_Mesh;
        }
    }

    [ClientRpc]
    public void RpcSetAppearance(CharacterAppearance[] appearances, int gender)
    {
        //for (int i = 0; i < appearances.Length; i++)
        //{
        //    Player.PlayerScriptable.appearance[i].id = appearances[i].id;
        //    Player.PlayerScriptable.appearance[i].color = appearances[i].color;
        //}
        //Debug.Log($"RpcSetAppearnce {appearances[i].id} & {appearances[i].color}");
        InitAppearance();
        SetAppearance(appearances, gender);
    }

    [Command(requiresAuthority = false)]
    public void CommandSetApearance()
    {
        RpcSetAppearance(Player.PlayerScriptable.appearance, Player.PlayerScriptable.gender);
    }
}
