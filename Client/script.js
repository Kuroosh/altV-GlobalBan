//----------------------------------//
///// VenoX Gaming & Fun 2019 © ///////
//////By Solid_Snake & VnX RL Crew////
////////www.venox-reallife.com////////
//----------------------------------//

import alt from "alt-client";
import * as game from "natives";


let localPlayer = alt.Player.local;
let kicked = false;

alt.log("VnXGlobalSystemsClient:Loaded");

alt.onServer("VnXGlobalSystemsClient:GetDiscordID", () => {
    try {
        if (!alt.Discord.currentUser)
            return null;
        let DiscordID = alt.Discord.currentUser.id.toString();
        alt.emitServer("VnXGlobalSystems:SetDiscordID", DiscordID);
    }
    catch{ }
});

alt.onServer("VnXGlobalSystemsClient:SetPedCanRagdoll", (bool) => {
    try { game.setPedCanRagdoll(localPlayer.scriptID, bool); }
    catch{ }
});
alt.onServer("VnXGlobalSystemsClient:SetProofs", (bool) => {
    try { game.setEntityProofs(localPlayer.scriptID, true, false, false, false, false, false, false, false); }
    catch{ }
});


alt.onServer("VnXGlobalSystemsClient:Kick", (reason) => {
    if (kicked) return;
    alt.log("~c~------------------------------------");
    alt.log("~r~-------- Kicked by Anticheat -------");
    alt.log("~c~------------------------------------");
	alt.emitServer('VnXGlobalSystems:KickPlayer');
    kicked = true;
});
alt.onServer("VnXGlobalSystemsClient:KickGlobal", () => {
    if (kicked) return;
    alt.log("~c~------------------------------------");
    alt.log("~r~--------        You´re Globally Banned by VenoX Global Systems!             -------");
    alt.log("~r~--------        E-Mail https://forum.altv.mp/profile/1466-solid_snake/      -------");
    alt.log("~b~~c~------------------------------------");
    alt.emitServer("VnXGlobalSystems:KickPlayer");
    kicked = true;
});
