﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PartyMovesPanel : PartyInformationUserInterface
{
    #region Variables

    private List<PartyInformationSlot> allSlots;

    #endregion

    #region Misccellaneous Methods

    public virtual List<PartyMember.MemberMove> GetMoves(PartyMember member)
    {
        return member.ActiveMoves; // Debug
    }

    public override void SetInformation(PartyMember member)
    {
        SetInformation(GetMoves(member));

        informationSlots[0].SetActive(true);
    }

    public void UpdatePosition(PartyMember member, int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        //GetMoves(member).Move(selectedValue, previousValue);

        PartyMember.MemberMove move = GetMoves(member)[previousValue];
        GetMoves(member).Remove(move);
        GetMoves(member).Insert(selectedValue, move);

        SetInformation(GetMoves(member));
        UpdateSelectedObject(selectedValue, increment);
    }

    public bool CanInsertMove(PartyMember member)
    {
        return (GetMoves(member).Count + 1) <= allSlots.Count;
    }

    public void InsertMove(PartyMember member, PartyMember.MemberMove move, int selectedValue)
    {
        GetMoves(member).Insert(selectedValue, move);
        SetInformation(GetMoves(member));
        UpdateSelectedObject(selectedValue, -1);
    }

    public int RemovetMove(PartyMember member, int selectedValue)
    {
        GetMoves(member).RemoveAt(selectedValue);
        SetInformation(GetMoves(member));


        int previousValue = selectedValue;

        if (previousValue > (MaxObjects - 1))
        {
            previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -1);
        }

        UpdateSelectedObject(previousValue, -1);

        return previousValue;
    }

    private void SetInformation(List<PartyMember.MemberMove> moves)
    {
        int counter = 0;

        informationSlots = allSlots.ToList();

        foreach (PartyInformationSlot slot in informationSlots)
        {
            slot.SetActive(true);
        }

        foreach (PartyMember.MemberMove move in moves)
        {
            informationSlots[counter].SetActive(false);
            informationSlots[counter].UpdateInformation(move);
            counter++;
        }

        for (int i = counter; i < informationSlots.Count; i++)
        {
            informationSlots[i].gameObject.SetActive(false);
        }

        informationSlots = RemoveInactiveObjects(informationSlots);
    }

    private List<PartyInformationSlot> RemoveInactiveObjects(List<PartyInformationSlot> source)
    {
        List<PartyInformationSlot> list = source;

        list.RemoveAll(panel => !panel.gameObject.activeSelf);

        return list;
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        allSlots = GetComponentsInChildren<PartyInformationSlot>().ToList();

        base.Awake();
    }

    #endregion
}

/*
    #region Variables

    public Flags flags = new Flags(false);

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isRearrangingMoves { get; set; }

        public Flags(bool isRearrangingMoves)
        {
            this.isRearrangingMoves = isRearrangingMoves;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public override void InitializePanel()
    {
        UpdateMoveInformation();

        informationSlots[0].SetActive(true);
    }

    public void UpdateMoveInformation()
    {
        int counter = 0;
        List<Pokemon.LearnedMove> moves = GetMoves(PartyManager.instance.selectedMember);

        informationSlots = transform.GetComponentsInChildren<PartyInformationSlots>();

        foreach (Pokemon.LearnedMove move in moves)
        {
            informationSlots[counter].SetActive(false);
            informationSlots[counter].GetComponentInChildren<MoveSlot>().UpdateInformation(move);
            counter++;
        }

        for (int i = counter; i < informationSlots.Length; i++)
        {
            informationSlots[i].gameObject.SetActive(false);
        }

        informationSlots = RemoveInactiveObjects(informationSlots);
    }

    protected virtual List<Pokemon.LearnedMove> GetMoves(int selectedMember)
    {
        Pokemon member = PartyManager.instance.party.playerParty[selectedMember];

        return member.activeMoves;
    }

    protected PartyInformationSlots[] RemoveInactiveObjects(PartyInformationSlots[] source)
    {
        List<PartyInformationSlots> list = source.ToList();

        list.RemoveAll(panel => !panel.gameObject.activeSelf);

        return list.ToArray();
    }

    private void UpdateMovePosition(int selectedMember, int selectedSlot, int increment)
    {
        int previousSlot = ExtensionMethods.IncrementInt(selectedSlot, 0, informationSlots.Length, increment);

        Pokemon.LearnedMove move = GetMoves(selectedMember)[previousSlot];
        GetMoves(selectedMember).Remove(move);
        GetMoves(selectedMember).Insert(selectedSlot, move);

        UpdateMoveInformation();
        UpdateSlot(selectedSlot, previousSlot);
    }

    protected override void GetInput()
    {
        if (!flags.isRearrangingMoves)
        {
            base.GetInput();

            if (Input.GetButtonDown("Remove") && PartyManager.instance.flags.isViewingAllMoves)
            {
                FindObjectOfType<PartyLearnedMovePanel>().SetActive(true);
                SetActive(false);
            }
        }
        else
        {
            bool hasInput;

            (selectedSlot, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, informationSlots.Length, selectedSlot);
            if (hasInput)
            {
                UpdateMovePosition(PartyManager.instance.selectedMember, selectedSlot, (int)Input.GetAxisRaw("Vertical"));
            }
        }

        if (Input.GetButtonDown("Interact") && !FindObjectOfType<PartyLearnedMovePanel>().isActive)
        {
            flags.isRearrangingMoves = !flags.isRearrangingMoves;
            StartCoroutine(PartyManager.instance.GetUserInterface().FadeIndicator(!flags.isRearrangingMoves));
            PartyManager.instance.GetUserInterface().UpdateIndicator(informationSlots, selectedSlot);
        }
    }

    #endregion
    */
