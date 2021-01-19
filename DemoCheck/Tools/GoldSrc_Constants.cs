using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCheck.Tools
{
    class GoldSrc_Constants
    {
        public enum ButtonsType : int
		{
            IN_ATTACK	    = (1 << 0),
            IN_JUMP		    = (1 << 1),
            IN_DUCK		    = (1 << 2),
            IN_FORWARD	    = (1 << 3),
            IN_BACK		    = (1 << 4),
            IN_USE		    = (1 << 5),
            IN_CANCEL	    = (1 << 6),
            IN_LEFT		    = (1 << 7),
            IN_RIGHT	    = (1 << 8),
            IN_MOVELEFT 	= (1 << 9),
            IN_MOVERIGHT    = (1 << 10),
            IN_ATTACK2	    = (1 << 11),
            IN_RUN          = (1 << 12),
            IN_RELOAD	    = (1 << 13),
            IN_ALT1		    = (1 << 14),
            IN_SCORE	    = (1 << 15)   // Used by client.dll for when scoreboard is held down
        };

		public enum WeaponIdType : ushort
		{
			WEAPON_NONE,
			WEAPON_P228,
			WEAPON_GLOCK,
			WEAPON_SCOUT,
			WEAPON_HEGRENADE,
			WEAPON_XM1014,
			WEAPON_C4,
			WEAPON_MAC10,
			WEAPON_AUG,
			WEAPON_SMOKEGRENADE,
			WEAPON_ELITE,
			WEAPON_FIVESEVEN,
			WEAPON_UMP45,
			WEAPON_SG550,
			WEAPON_GALIL,
			WEAPON_FAMAS,
			WEAPON_USP,
			WEAPON_GLOCK18,
			WEAPON_AWP,
			WEAPON_MP5N,
			WEAPON_M249,
			WEAPON_M3,
			WEAPON_M4A1,
			WEAPON_TMP,
			WEAPON_G3SG1,
			WEAPON_FLASHBANG,
			WEAPON_DEAGLE,
			WEAPON_SG552,
			WEAPON_AK47,
			WEAPON_KNIFE,
			WEAPON_P90,
			WEAPON_SHIELDGUN = 99
		};
	}
}
