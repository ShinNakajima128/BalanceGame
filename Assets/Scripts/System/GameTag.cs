using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class GameTag
{
    #region property
    public static string Player => _player;
    public static string Item => _item;
    public static string Stage => _stage;
    #endregion

    #region private
    private static string _player = "Player";
    private static string _item = "Item";
    private static string _stage = "Stage";
    #endregion
}
