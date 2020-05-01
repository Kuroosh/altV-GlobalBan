declare module "alt-client" {
  type FileEncoding = "utf-8" | "utf-16" | "binary";

  /**
   * @deprecated
   */
  export interface DiscordOAuth2Token {
    readonly token: string
    readonly expires: number;
    readonly scopes: string;
  }

  export interface DiscordUser {
    readonly id: string;
    readonly name: string;
    readonly discriminator: string;
    readonly avatar: string;
  }

  export interface Vector2 {
    /** x component of Vector2 */
    readonly x: number;

    /** y component of Vector2 */
    readonly y: number;
  }

  /** Class representing Vector3 */
  export class Vector3 {
    /** x component of Vector3 */
    public readonly x: number;
    /** y component of Vector3 */
    public readonly y: number;
    /** z component of Vector3 */
    public readonly z: number;

    /**
     * Create a Vector3
     *
     * @param x x component
     * @param y y component
     * @param z z component
     */
    constructor(x: number, y: number, z: number);
  }

  /** Class representing RGBA */
  export class RGBA {
    /** r component of RGBA */
    public r: number;
    /** g component of RGBA */
    public g: number;
    /** b component of RGBA */
    public b: number;
    /** a component of RGBA */
    public a: number;

    constructor(r: number, g: number, b: number, a: number);
  }

  /** Base class for any alt:V object */
  export class BaseObject {
    /** Object type */
    public readonly type: number;

    /**
     * Value true if object is valid
     */
    public readonly valid: boolean;

    /** Destroy object */
    public destroy(): void;

    /**
     * Get meta-data value
     *
     * @param key key
     * @returns value
     */
    public getMeta(key: string): any;

    /**
     * Set meta-data value
     *
     * @param key key
     * @param value value
     */
    public setMeta(key: string, value: any): void;
  }

  /** Base class for any object that exists in game world */
  export class WorldObject extends BaseObject {
    /**
     * Object position
     *
     * @remarks Property is readonly for network entities
     */
    public pos: Vector3;
  }

  /** Base class for network entities */
  export class Entity extends WorldObject {
    /** Entity unique id */
    public readonly id: number;

    /** Internal game id that can be used in native calls */
    public readonly scriptID: number;

    /** Hash of entity model */
    public readonly model: number;

    /** Entity rotation */
    public readonly rot: Vector3;

    public static getByID(id: number): Entity | null;

    public static getByScriptID(scriptID: number): Entity | null;

    /**
     * Get synced meta-data value
     *
     * @param key key
     * @returns value
     */
    public getSyncedMeta(key: string): any;
  }

  /** Class representing alt:V Player */
  export class Player extends Entity {
    /** Array with all players */
    public static readonly all: Array<Player>;

    /** Local player */
    public static readonly local: Player;

    /** Player talking state */
    public readonly isTalking: boolean;

    /** Name */
    public readonly name: string;

    /** Player's vehicle, null if player is not in any vehicle */
    public readonly vehicle: Vehicle | null;
  }

  /** Class representing alt:V Vehicle */
  export class Vehicle extends Entity {
    /** Array with all vehicles */
    public static readonly all: Array<Vehicle>;

    /** Vehicle gear */
    public gear: number;

    /** Vehicle RPM [0, 1] */
    public readonly rpm: number;

    /** Vehicle wheel speed */
    public readonly speed: number;

    /** Vehicle wheel speed vector */
    public readonly speedVector: Vector3;

    /** Vehicle wheel count */
    public readonly wheelsCount: number;
  }

  /** Class representing web view */
  export class WebView extends BaseObject {
    /** View visibility state */
    public isVisible: boolean;
    /** View URL */
    public url: string;

    /**
     * Creates a fullscreen WebView
     *
     * @param url URL
     * @param isOverlay true to render as overlay, false to render on game's GUI stage
     */
    constructor(url: string, isOverlay?: boolean);

    /**
     * Creates a WebView rendered on game object
     *
     * @param url URL
     * @param propHash hash of object to render on
     * @param targetTexture name of object's texture to replace
     */
    constructor(url: string, propHash: number, targetTexture: string);

    public emit(eventName: string, ...args: any[]): void;

    public focus(): void;

    public off(eventName: string, listener: (...args: any[]) => void): void;

    public on(eventName: string, listener: (...args: any[]) => void): void;

    public unfocus(): void;
  }

  export class Blip extends WorldObject {
    public alpha: number;
    public asMissionCreator: boolean;
    public bright: boolean;
    public category: number;
    public color: number;
    public crewIndicatorVisible: boolean;
    public flashInterval: number;
    public flashTimer: number;
    public flashes: boolean;
    public flashesAlternate: boolean;
    public friendIndicatorVisible: boolean;
    public friendly: boolean;
    public gxtName: string;
    public heading: number;
    public headingIndicatorVisible: boolean;
    public highDetail: boolean;
    public name: string;
    public number: number;
    public outlineIndicatorVisible: boolean;
    public priority: number;
    public pulse: boolean;
    public route: boolean;
    public routeColor: number;
    public scale: number;
    public secondaryColor: number;
    public shortRange: boolean;
    public showCone: boolean;
    public shrinked: boolean;
    public sprite: number;
    public tickVisible: boolean;

    public fade(opacity: number, duration: number): void;
  }

  export class AreaBlip extends Blip {
    constructor(x: number, y: number, z: number, width: number, height: number);
  }

  export class RadiusBlip extends Blip {
    constructor(x: number, y: number, z: number, radius: number);
  }

  export class PointBlip extends Blip {
    constructor(x: number, y: number, z: number);
  }

  export class HandlingData {
    public acceleration: number;
    public antiRollBarBiasFront: number;
    public antiRollBarBiasRear: number;
    public antiRollBarForce: number;
    public brakeBiasFront: number;
    public brakeBiasRear: number;
    public breakForce: number;
    public camberStiffnesss: number;
    public centreOfMassOffset: Vector3;
    public clutchChangeRateScaleDownShift: number;
    public clutchChangeRateScaleUpShift: number;
    public collisionDamageMult: number;
    public damageFlags: number;
    public deformationDamageMult: number;
    public downforceModifier: number;
    public driveBiasFront: number;
    public driveInertia: number;
    public driveMaxFlatVel: number;
    public engineDamageMult: number;
    public handBrakeForce: number;
    public handlingFlags: number;
    public readonly handlingNameHash: number;
    public inertiaMultiplier: Vector3;
    public initialDragCoeff: number;
    public initialDriveForce: number;
    public initialDriveGears: number;
    public initialDriveMaxFlatVel: number;
    public lowSpeedTractionLossMult: number;
    public mass: number;
    public modelFlags: number;
    public monetaryValue: number;
    public oilVolume: number;
    public percentSubmerged: number;
    public percentSubmergedRatio: number;
    public petrolTankVolume: number;
    public rollCentreHeightFront: number;
    public rollCentreHeightRear: number;
    public seatOffsetDistX: number;
    public seatOffsetDistY: number;
    public seatOffsetDistZ: number;
    public steeringLock: number;
    public steeringLockRatio: number;
    public suspensionBiasFront: number;
    public suspensionBiasRear: number;
    public suspensionCompDamp: number;
    public suspensionForce: number;
    public suspensionLowerLimit: number;
    public suspensionRaise: number;
    public suspensionReboundDamp: number;
    public suspensionUpperLimit: number;
    public tractionBiasFront: number;
    public tractionBiasRear: number;
    public tractionCurveLateral: number;
    public tractionCurveLateralRatio: number;
    public tractionCurveMax: number;
    public tractionCurveMaxRatio: number;
    public tractionCurveMin: number;
    public tractionCurveMinRatio: number;
    public tractionLossMult: number;
    public tractionSpringDeltaMax: number;
    public tractionSpringDeltaMaxRatio: number;
    public unkFloat1: number;
    public unkFloat2: number;
    public unkFloat4: number;
    public unkFloat5: number;
    public weaponDamageMult: number;

    public static getForModel(modelHash: number): HandlingData;
  }

  export class MapZoomData {
    public fZoomScale: number;
    public fZoomSpeed: number;
    public fScrollSpeed: number;
    public vTilesX: number;
    public vTilesY: number;

    public static get(zoomData: string): MapZoomData;

    public static resetAll(): void;

    public reset(): void;
  }

  export class LocalStorage {
    public static get(): LocalStorage;

    public delete(key: string): void;

    public deleteAll(): void;

    public get(key: string): any;

    public save(): void;

    public set(key: string, value: any): void;
  }

  export class MemoryBuffer {
    constructor(size: number);

    public byte(offset: number, value: number): number;

    public double(offset: number, value: number): number;

    public float(offset: number, value: number): number;

    public int(offset: number, value: number): number;

    public long(offset: number, value: number): bigint;

    public short(offset: number, value: number): number;

    public string(offset: number, value: number): string;

    public ubyte(offset: number, value: number): number;

    public uint(offset: number, value: number): number;

    public ulong(offset: number, value: number): bigint;

    public ushort(offset: number, value: number): number;

    public free(): boolean;
  }

  export class Discord {
    public static readonly currentUser: DiscordUser | null;

    /**
     * @deprecated
     */
    public static requestOAuth2Token(): Promise<DiscordOAuth2Token>;
  }

  export class File {
    public static exists(filename: string): boolean;

    public static read(filename: string, encoding?: FileEncoding): string | ArrayBuffer;
  }

  export function addGxtText(key: string, value: string): void;

  export function beginScaleformMovieMethodMinimap(methodName: string): boolean;

  export function clearEveryTick(handle: number): void;

  export function clearInterval(handle: number): void;

  export function clearNextTick(handle: number): void;

  export function clearTimeout(handle: number): void;

  export function clearTimer(handle: number): void;

  export function disableVoiceActivation(): void;

  export function disableVoiceInput(): boolean;

  export function disableVoiceTest(): boolean;

  /**
   * @deprecated Use {@link Discord.currentUser}
   */
  export function discordInfo(): Object | null;

  /**
   * @deprecated Use {@link Discord.requestOAuth2Token}
   */
  export function discordRequestOAuth2(): boolean;

  export function emit(name: string, ...args: any[]): void;

  export function emitServer(name: string, ...args: any[]): void;

  export function enableVoiceActivation(activateOn: number, activationTime: number): void;

  export function enableVoiceInput(): boolean;

  export function enableVoiceTest(): boolean;

  export function everyTick(handler: () => void): number;

  export function gameControlsEnabled(): boolean;

  export function getCursorPos(): Vector2;

  /**
   * @deprecated Use {@link Discord.requestOAuth2Token}
   */
  export function getDiscordOAuth2Result(): any;

  export function getGxtText(key: string): string;

  export function getLicenseHash(): string;

  /**
   * @deprecated Use {@link Player.local}
   */
  export function getLocalPlayer(): Player;

  export function getLocale(): string;

  export function getMicLevel(): number;

  export function getMsPerGameMinute(): number;

  export function getStat(statName: string): number;

  export function hash(str: string): number;

  export function initVoice(bitrate: number): boolean;

  /**
   * @deprecated Use {@link Discord.currentUser}
   */
  export function isDiscordInfoReady(): boolean;

  /**
   * @deprecated Use {@link Discord.requestOAuth2Token}
   */
  export function isDiscordOAuth2Accepted(): boolean;

  /**
   * @deprecated Use {@link Discord.requestOAuth2Token}
   */
  export function isDiscordOAuth2Finished(): boolean;

  export function isInSandbox(): boolean;

  export function isTextureExistInArchetype(modelHash: number, modelName: string): boolean;

  export function loadModel(modelHash: number): void;

  export function loadModelAsync(modelHash: number): void;

  export function log(...args: any[]): void;

  export function logError(...args: any[]): void;

  export function logWarning(...args: any[]): void;

  export function nextTick(handler: () => void): number;

  export function off(eventName: string, listener: (...args: any[]) => void): void;

  export function offServer(eventName: string, listener: (...args: any[]) => void): void;

  export function on(eventName: string, listener: (...args: any[]) => void): void;
  export function on(eventName: "anyResourceError", listener: (resourceName: string) => void): void;
  export function on(eventName: "anyResourceStart", listener: (resourceName: string) => void): void;
  export function on(eventName: "anyResourceStop", listener: (resourceName: string) => void): void;
  export function on(eventName: "connectionComplete", listener: () => void): void;
  export function on(eventName: "consoleCommand", listener: (name: string, ...args: string[]) => void): void;
  export function on(eventName: "disconnect", listener: () => void): void;
  export function on(eventName: "gameEntityCreate", listener: (entity: Entity) => void): void;
  export function on(eventName: "gameEntityDestroy", listener: (entity: Entity) => void): void;
  export function on(eventName: "keydown", listener: (key: number) => void): void;
  export function on(eventName: "keyup", listener: (key: number) => void): void;
  export function on(eventName: "removeEntity", listener: (object: BaseObject) => void): void;
  export function on(eventName: "resourceStart", listener: (errored: boolean) => void): void;
  export function on(eventName: "resourceStop", listener: () => void): void;
  export function on(eventName: "syncedMetaChange", listener: (entity: Entity, key: string, value: any) => void): void;

  export function onServer(eventName: string, listener: (...args: any[]) => void): void;

  export function removeGxtText(key: string): void;

  export function removeIpl(iplName: string): void;

  export function requestIpl(iplName: string): void;

  /**
   * Output is saved to screenshots folder in root directory
   *
   * @remarks Only available in sandbox mode
   * @param stem Filename without extension
   * @return Return is dependent on the success of the operation
   */
  export function saveScreenshot(stem: string): boolean;

  export function resetStat(statName: string): void;

  export function setCamFrozen(state: boolean): void;

  export function setCursorPos(pos: Vector2): void;

  export function setInterval(handler: () => void, time: number): number;

  export function setMicGain(micGain: number): void;

  export function setModel(modelName: string): void;

  export function setMsPerGameMinute(ms: number): void;

  export function setNoiseSuppressionStatus(state: boolean): void;

  export function setStat(statName: string, value: number): void;

  export function setTimeout(handler: () => void, time: number): number;

  export function setWeatherCycle(weathers: Array<any>, multipliers: Array<any>): void;

  export function setWeatherSyncActive(isActive: boolean): void;

  export function showCursor(state: boolean): void;

  export function toggleGameControls(state: boolean): void;
}