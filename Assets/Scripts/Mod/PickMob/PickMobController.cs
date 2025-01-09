using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod.XMAP
{
    public class PickMobController
    {
        public static void Update()
        {
            if (!IsWaiting())
            {
                Char @char = Char.myCharz();

                // Status me
                // 1: stand
                // 2: run
                // 3: fly
                // 10: fly with mount
                // 14: dead

                if (@char.statusMe != 14 && @char.cHP > 0)
                {
                    // Buff HP
                    if (GameScr.hpPotion >= 1 && (@char.cHP <= @char.cHPFull * PickMob.HpBuff / 100 || @char.cMP <= @char.cMPFull * PickMob.MpBuff / 100))
                    {
                        GameScr.gI().doUseHP();
                    }

                    // Pick item
                    if (PickMob.IsAutoPickItems)
                    {
                        if (IsPickingItems)
                        {
                            if (IndexItemPick >= ItemPicks.Count)
                            {
                                IsPickingItems = false;
                                Wait(100);
                                return;
                            }
                            ItemMap itemMap = ItemPicks[IndexItemPick];
                            if (GameScr.vItemMap.contains(itemMap))
                            {
                                Service.gI().pickItem(itemMap.itemMapID);
                                itemMap.countAutoPick++;
                            }
                            Wait(500);
                            IndexItemPick++;
                        }
                        ItemPicks.Clear();
                        IndexItemPick = 0;
                        for (int i = 0; i < GameScr.vItemMap.size(); i++)
                        {
                            ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(i);
                            if (GetTypePickItem(itemMap2) != TpyePickItem.CanNotPickItem)
                            {
                                ItemPicks.Add(itemMap2);
                            }
                        }
                        if (ItemPicks.Count > 0)
                        {
                            IsPickingItems = true;
                            return;
                        }
                    }

                    // Tan sat mob
                    bool isTDLT = ItemTime.isExistItem(4387);
                    if (PickMob.tanSat)
                    {
                        if (@char.isCharge)
                        {
                            Wait(100);
                        }
                        else
                        {
                            if (@char.mobFocus != null && !IsMobTanSat(@char.mobFocus))
                            {
                                @char.mobFocus = null;
                            }
                            if (@char.mobFocus == null)
                            {
                                @char.mobFocus = GetMobTanSat();
                                if (isTDLT && @char.mobFocus != null)
                                {
                                    if (PickMob.telePem)
                                    {
                                        if (Math.abs(@char.mobFocus.xFirst - @char.cx) >= 20 || Math.abs(@char.mobFocus.yFirst - @char.cy) >= 20)
                                        {
                                            ModFunc.GI().MoveTo(@char.mobFocus.xFirst, @char.mobFocus.yFirst);
                                        }
                                        return;
                                    }
                                    @char.cx = @char.mobFocus.xFirst;
                                    @char.cy = @char.mobFocus.yFirst;
                                    Service.gI().charMove();
                                }
                            }
                            if (@char.mobFocus != null)
                            {
                                if (@char.skillInfoPaint() == null)
                                {
                                    Skill skillAttack = GetSkillAttack2();
                                    if (skillAttack != null && CanUseSkill(skillAttack))
                                    {
                                        Mob mobFocus = @char.mobFocus;
                                        mobFocus.x = mobFocus.xFirst;
                                        mobFocus.y = mobFocus.yFirst;
                                        if (Char.myCharz().myskill != skillAttack)
                                        {
                                            GameScr.gI().doSelectSkill(skillAttack, true);
                                        }
                                        if (Res.distance(mobFocus.xFirst, mobFocus.yFirst, @char.cx, @char.cy) <= 48)
                                        {
                                            if (GameCanvas.gameTick % 50 == 0 && Mob.arrMobTemplate[Char.myCharz().mobFocus.templateId].type == 4)
                                            {
                                                ModFunc.GI().MoveTo(mobFocus.xFirst, mobFocus.yFirst + 1);
                                            }
                                            if (skillAttack.template.isAttackSkill())
                                            {
                                                ModFunc.GI().AttackMob(mobFocus);
                                                ModFunc.GI().SetUsedSkill(@char.myskill);
                                            }
                                            else if (skillAttack.template.isUseAlone())
                                            {
                                                ModFunc.GI().DoDoubleClickToObj(mobFocus);
                                            }
                                        }
                                        else
                                        {
                                            if (PickMob.telePem)
                                            {
                                                if (Math.abs(mobFocus.xFirst - @char.cx) >= 20 || Math.abs(mobFocus.yFirst - @char.cy) >= 20)
                                                {
                                                    ModFunc.GI().MoveTo(mobFocus.xFirst, mobFocus.yFirst);
                                                }
                                                return;
                                            }
                                            Move(mobFocus.xFirst, mobFocus.yFirst);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!isTDLT)
                                {
                                    Mob mobNext = GetMobNext();
                                    if (mobNext != null)
                                    {
                                        Char.myCharz().FocusManualTo(mobNext);
                                        if (PickMob.telePem)
                                        {
                                            if (Math.abs(mobNext.xFirst - @char.cx) >= 20 || Math.abs(mobNext.yFirst - @char.cy) >= 20)
                                            {
                                                ModFunc.GI().MoveTo(mobNext.xFirst, mobNext.yFirst);
                                            }
                                            return;
                                        }
                                        Move(mobNext.xFirst, mobNext.yFirst);
                                    }
                                }
                            }
                            Wait(100);
                        }
                    }

                    // Tan sat player
                    else if (PickMob.tsPlayer)
                    {
                        if (@char.isCharge)
                        {
                            Wait(100);
                        }
                        else
                        {
                            if (@char.charFocus != null && !IsCharTanSat(@char.charFocus))
                            {
                                @char.charFocus = null;
                            }
                            if (@char.charFocus == null)
                            {
                                @char.charFocus = GetCharTanSat();
                                if (@char.charFocus != null)
                                {
                                    if (PickMob.telePem)
                                    {
                                        if (Math.abs(@char.charFocus.cx - @char.cx) >= 20 || Math.abs(@char.charFocus.cy - @char.cy) >= 20)
                                        {
                                            ModFunc.GI().MoveTo(@char.charFocus.cx, @char.charFocus.cy);
                                        }
                                        return;
                                    }
                                    @char.cx = @char.charFocus.cx;
                                    @char.cy = @char.charFocus.cy;
                                    Service.gI().charMove();
                                }
                            }
                            if (@char.charFocus != null)
                            {
                                if (@char.skillInfoPaint() == null)
                                {
                                    Skill skillAttack = GetSkillAttack2();
                                    if (skillAttack != null && !skillAttack.paintCanNotUseSkill)
                                    {
                                        Char charFocus = @char.charFocus;
                                        if (@char.myskill != skillAttack)
                                        {
                                            GameScr.gI().doSelectSkill(skillAttack, true);
                                        }
                                        if (Res.distance(charFocus.cx, charFocus.cy, @char.cx, @char.cy) <= 48)
                                        {
                                            ModFunc.GI().DoDoubleClickToObj(charFocus);
                                        }
                                        else
                                        {
                                            if (PickMob.telePem)
                                            {
                                                if (Math.abs(charFocus.cx - @char.cx) >= 20 || Math.abs(charFocus.cy - @char.cy) >= 20)
                                                {
                                                    ModFunc.GI().MoveTo(charFocus.cx, charFocus.cy);
                                                }
                                                return;
                                            }
                                            Move(charFocus.cx, charFocus.cy);
                                        }
                                    }
                                }
                            }
                            Wait(100);
                        }
                    }
                }
            }
        }

        public static void Move(int x, int y)
        {
            Char @char = Char.myCharz();
            bool flag = !PickMob.vuotDiaHinh;
            if (flag)
            {
                @char.currentMovePoint = new MovePoint(x, y);
            }
            else
            {
                int[] pointYsdMax = GetPointYsdMax(@char.cx, x);
                bool flag2 = pointYsdMax[1] >= y || (pointYsdMax[1] >= @char.cy && (@char.statusMe == 2 || @char.statusMe == 1));
                if (flag2)
                {
                    pointYsdMax[0] = x;
                    pointYsdMax[1] = y;
                }
                @char.currentMovePoint = new MovePoint(pointYsdMax[0], pointYsdMax[1]);
            }
        }

        private static TpyePickItem GetTypePickItem(ItemMap itemMap)
        {
            TpyePickItem result;
            Char @char = Char.myCharz();
            bool canPick = itemMap.playerId == @char.charID || itemMap.playerId == -1 || PickMob.IsPickItemsAll;
            if (!canPick)
            {
                result = TpyePickItem.CanNotPickItem;
            }
            else
            {
                if (PickMob.IsLimitTimesPickItem && itemMap.countAutoPick > PickMob.TimesAutoPickItemMax)
                {
                    result = TpyePickItem.CanNotPickItem;
                }
                else
                {
                    if (!FilterItemPick(itemMap))
                    {
                        result = TpyePickItem.CanNotPickItem;
                    }
                    else
                    {
                        if (PickMob.IsPickItemsDis || Res.abs(@char.cx - itemMap.xEnd) < 100 && Res.abs(@char.cy - itemMap.yEnd) < 100)
                        {
                            result = TpyePickItem.PickItemNormal;
                        }
                        else
                        {
                            if (ItemTime.isExistItem(4387))
                            {
                                result = TpyePickItem.PickItemTDLT;
                            }
                            else
                            {
                                if (PickMob.tanSat)
                                {
                                    result = TpyePickItem.PickItemTanSat;
                                }
                                else
                                {
                                    result = TpyePickItem.CanNotPickItem;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static bool FilterItemPick(ItemMap itemMap)
        {
            return (PickMob.IdItemPicks.Count == 0 || PickMob.IdItemPicks.Contains(itemMap.template.id))
                && (PickMob.IdItemBlocks.Count == 0 || !PickMob.IdItemBlocks.Contains(itemMap.template.id))
                && (PickMob.TypeItemPicks.Count == 0 || PickMob.TypeItemPicks.Contains(itemMap.template.type))
                && (PickMob.TypeItemBlock.Count == 0 || !PickMob.TypeItemBlock.Contains(itemMap.template.type));
        }

        private static Mob GetMobTanSat()
        {
            Mob result = null;
            int num = int.MaxValue;
            Char @char = Char.myCharz();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                int num2 = (mob.xFirst - @char.cx) * (mob.xFirst - @char.cx) + (mob.yFirst - @char.cy) * (mob.yFirst - @char.cy);
                bool flag = IsMobTanSat(mob) && num2 < num;
                if (flag)
                {
                    result = mob;
                    num = num2;
                }
            }
            return result;
        }

        private static Char GetCharTanSat()
        {
            Char result = null;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char c = (Char) GameScr.vCharInMap.elementAt(i);
                if (IsCharTanSat(c))
                {
                    result = c;
                }
            }
            return result;
        }

        private static Mob GetMobNext()
        {
            Mob result = null;
            long num = mSystem.currentTimeMillis();
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                bool flag = IsMobNext(mob) && mob.lastDie < num;
                if (flag)
                {
                    result = mob;
                    num = mob.lastDie;
                }
            }
            return result;
        }

        private static bool IsMobTanSat(Mob mob)
        {
            bool result;
            if (mob.status == 0 || mob.status == 1 || mob.hp <= 0 || mob.isMobMe)
            {
                result = false;
            }
            else
            {
                bool flag = PickMob.neSieuQuai && !ItemTime.isExistItem(4387);
                result = ((mob.levelBoss == 0 || !flag) && FilterMobTanSat(mob));
            }
            return result;
        }

        private static bool IsCharTanSat(Char c)
        {
            return Char.myCharz().isMeCanAttackOtherPlayer(c);
        }

        private static bool IsMobNext(Mob mob)
        {
            bool isMobMe = mob.isMobMe;
            bool result;
            if (isMobMe)
            {
                result = false;
            }
            else
            {
                bool flag = !FilterMobTanSat(mob);
                if (flag)
                {
                    result = false;
                }
                else
                {
                    bool flag2 = PickMob.neSieuQuai && !ItemTime.isExistItem(4387) && mob.getTemplate().hp >= 3000;
                    if (flag2)
                    {
                        bool flag3 = mob.levelBoss != 0;
                        if (flag3)
                        {
                            Mob mob2 = null;
                            bool flag4 = false;
                            for (int i = 0; i < GameScr.vMob.size(); i++)
                            {
                                mob2 = (Mob)GameScr.vMob.elementAt(i);
                                bool flag5 = mob2.countDie == 10 && (mob2.status == 0 || mob2.status == 1);
                                if (flag5)
                                {
                                    flag4 = true;
                                    break;
                                }
                            }
                            bool flag6 = !flag4;
                            if (flag6)
                            {
                                return false;
                            }
                            mob.lastDie = mob2.lastDie;
                        }
                        else
                        {
                            bool flag7 = mob.countDie == 10 && (mob.status == 0 || mob.status == 1);
                            if (flag7)
                            {
                                return false;
                            }
                        }
                    }
                    result = true;
                }
            }
            return result;
        }

        private static bool FilterMobTanSat(Mob mob)
        {
            return (PickMob.IdMobsTanSat.Count == 0 || PickMob.IdMobsTanSat.Contains(mob.mobId)) && (PickMob.TypeMobsTanSat.Count == 0 || PickMob.TypeMobsTanSat.Contains(mob.templateId)) && !mob.isMobMe;
        }

        private static Skill GetSkillAttack()
        {
            Skill skill = null;
            SkillTemplate skillTemplate = new();
            foreach (sbyte id in PickMob.IdSkillsTanSat)
            {
                skillTemplate.id = id;
                Skill skill2 = Char.myCharz().getSkill(skillTemplate);
                if (IsSkillBetter(skill2, skill))
                {
                    skill = skill2;
                }
            }
            return skill;
        }

        private static Skill GetSkillAttack2()
        {
            Skill skill = null;

            // Skill on screen
            // Except skill type 2, 4; skill id 10, 11, 14, 23
            Skill[] skills = GameCanvas.isTouch ? GameScr.onScreenSkill : GameScr.keySkill;
            for (int i = 0; i < skills.Length; i++)
            {
                Skill s = skills[i];
                if (s == null
                    || s.template.id == 10
                    || s.template.id == 11
                    || s.template.id == 14
                    || s.template.id == 23
                    || s.template.isBuffToPlayer()
                    || s.template.isSkillSpec())
                {
                    continue;
                }
                if (IsSkillBetter(s, skill))
                {
                    skill = s;
                }
            }
            return skill;
        }

        private static bool IsSkillBetter(Skill SkillBetter, Skill skill)
        {
            bool result;
            if (SkillBetter == null)
            {
                result = false;
            }
            else if (skill == null)
            {
                result = true;
            }
            else
            {
                if (!CanUseSkill(SkillBetter))
                {
                    result = false;
                }
                else if (!CanUseSkill(skill))
                {
                    result = true;
                }
                else
                {
                    bool flag3 = (SkillBetter.template.id == 17 && skill.template.id == 2) || (SkillBetter.template.id == 9 && skill.template.id == 0);
                    result = skill.coolDown < SkillBetter.coolDown || flag3;
                }
            }
            return result;
        }

        private static bool CanUseSkill(Skill skill)
        {
            // Check cooldown & mana
            if (mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown + 25L)
            {
                skill.paintCanNotUseSkill = false;
            }
            return !skill.paintCanNotUseSkill && Char.myCharz().cMP >= GetManaUseSkill(skill);
        }

        private static double GetManaUseSkill(Skill skill)
        {
            bool flag = skill.template.manaUseType == 2;
            double result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = skill.template.manaUseType == 1;
                if (flag2)
                {
                    result = skill.manaUse * Char.myCharz().cMPFull / 100;
                }
                else
                {
                    result = skill.manaUse;
                }
            }
            return result;
        }

        public static int GetYsd(int xsd)
        {
            Char @char = Char.myCharz();
            int num = TileMap.pxh;
            int result = -1;
            for (int i = 24; i < TileMap.pxh; i += 24)
            {
                bool flag = TileMap.tileTypeAt(xsd, i, 2);
                if (flag)
                {
                    int num2 = Res.abs(i - @char.cy);
                    bool flag2 = num2 < num;
                    if (flag2)
                    {
                        num = num2;
                        result = i;
                    }
                }
            }
            return result;
        }

        private static int[] GetPointYsdMax(int xStart, int xEnd)
        {
            int num = TileMap.pxh;
            int num2 = -1;
            bool flag = xStart > xEnd;
            if (flag)
            {
                for (int i = xEnd; i < xStart; i += 24)
                {
                    int ysd = GetYsd(i);
                    bool flag2 = ysd < num;
                    if (flag2)
                    {
                        num = ysd;
                        num2 = i;
                    }
                }
            }
            else
            {
                for (int j = xEnd; j > xStart; j -= 24)
                {
                    int ysd2 = GetYsd(j);
                    bool flag3 = ysd2 < num;
                    if (flag3)
                    {
                        num = ysd2;
                        num2 = j;
                    }
                }
            }
            return new int[]
            {
                num2,
                num
            };
        }

        public static void Wait(int time)
        {
            IsWait = true;
            TimeStartWait = mSystem.currentTimeMillis();
            TimeWait = time;
        }

        public static bool IsWaiting()
        {
            if (IsWait && mSystem.currentTimeMillis() - TimeStartWait >= TimeWait)
            {
                IsWait = false;
            }
            return IsWait;
        }

        private static readonly sbyte[] IdSkillsMelee = new sbyte[]
        {
            0,
            9,
            2,
            17,
            4
        };

        private static readonly sbyte[] IdSkillsCanNotAttack = new sbyte[]
        {
            10,
            11,
            14,
            23,
            7
        };

        private static readonly PickMobController _Instance = new();

        public static bool IsPickingItems;

        private static bool IsWait;

        private static long TimeStartWait;

        private static long TimeWait;

        public static List<ItemMap> ItemPicks = new List<ItemMap>();

        private static int IndexItemPick = 0;

        private enum TpyePickItem
        {
            CanNotPickItem,
            PickItemNormal,
            PickItemTDLT,
            PickItemTanSat
        }
    }
}
