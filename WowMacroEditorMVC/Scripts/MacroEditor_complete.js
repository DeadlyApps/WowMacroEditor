/// <reference path="MacroEditor_codemirror.js" />
/// <reference path="MacroEditor_Mode.js" />
/// <reference path="MacroEditor_overlay.js" />

(function () {
    // Minimal event-handling wrapper.
    function stopEvent() {
        if (this.preventDefault) { this.preventDefault(); this.stopPropagation(); }
        else { this.returnValue = false; this.cancelBubble = true; }
    }
    function addStop(event) {
        if (!event.stop) event.stop = stopEvent;
        return event;
    }
    function connect(node, type, handler) {
        function wrapHandler(event) { handler(addStop(event || window.event)); }
        if (typeof node.addEventListener == "function")
            node.addEventListener(type, wrapHandler, false);
        else
            node.attachEvent("on" + type, wrapHandler);
    }

    function forEach(arr, f) {
        for (var i = 0, e = arr.length; i < e; ++i) f(arr[i]);
    }

    var countDiv = $(".macro-length");
    var editor = CodeMirror.fromTextArea(document.getElementById("MacroText"), {
        lineNumbers: true,
        onKeyEvent: function (i, e) {
            // Hook into ctrl-space
            if (e.keyCode == 32 && (e.ctrlKey || e.metaKey) && !e.altKey) {
                e.stop();
                return startComplete();
            }
            countDiv.html((255 - editor.getValue().length).toString());
        }
    });

    function startComplete() {
        // We want a single cursor position.
        if (editor.somethingSelected()) return;
        // Find the token at the cursor
        var cur = editor.getCursor(false), token = editor.getTokenAt(cur), tprop = token;
        // If it's not a 'word-style' token, ignore the token.

        // If it is a property, find out what it is a property of.
        if (tprop.className != null) {
            if (!context) var context = [];
            context.push(tprop);
        }
        var completions = getCompletions(token, context);
        if (!completions.length) return;
        function insert(str) {
            if (token.className == "bracket-command") {
                var values = token.string.replace("[", "").replace("]", "").split(",");
                values[values.length - 1] = str;
                str = "[";
                for (var i = 0; i < values.length; i++) {
                    if (i > 0)
                        str += ",";
                    str += values[i];
                }
                str += "]";
            }
            if (token.string === ' ' || token.string === ',')
                editor.replaceRange(str, { line: cur.line, ch: token.start + 1 }, { line: cur.line, ch: token.end + 1 });
            else
                editor.replaceRange(str, { line: cur.line, ch: token.start }, { line: cur.line, ch: token.end });
        }
        // When there is only one completion, use it directly.
        if (completions.length == 1) { insert(completions[0]); return true; }

        // Build the select widget
        var complete = document.createElement("div");
        complete.className = "completions";
        var sel = complete.appendChild(document.createElement("select"));
        sel.multiple = true;
        for (var i = 0; i < completions.length; ++i) {
            var opt = sel.appendChild(document.createElement("option"));
            opt.appendChild(document.createTextNode(completions[i]));
        }
        sel.firstChild.selected = true;
        sel.size = Math.min(10, completions.length);
        var pos = editor.cursorCoords();
        complete.style.left = pos.x + "px";
        complete.style.top = pos.yBot + "px";
        document.body.appendChild(complete);
        // Hack to hide the scrollbar.
        if (completions.length <= 10)
            complete.style.width = (sel.clientWidth - 1) + "px";

        var done = false;
        function close() {
            if (done) return;
            done = true;
            complete.parentNode.removeChild(complete);
        }
        function pick() {
            insert(sel.options[sel.selectedIndex].value);
            close();
            setTimeout(function () { editor.focus(); }, 50);
        }
        connect(sel, "blur", close);
        connect(sel, "keydown", function (event) {
            var code = event.keyCode;
            // Enter and space
            if (code == 13 || code == 32) { event.stop(); pick(); }
            // Escape
            else if (code == 27) { event.stop(); close(); editor.focus(); }
            else if (code != 38 && code != 40) { close(); editor.focus(); setTimeout(startComplete, 50); }
        });
        connect(sel, "dblclick", pick);

        sel.focus();
        // Opera sometimes ignores focusing a freshly created node
        if (window.opera) setTimeout(function () { if (!done) sel.focus(); }, 100);
        return true;
    }

    var PoundKeywords = ("#show #showtooltip").split(" ");
    var slashKeywords = ("/assist /cancelaura /cancelform /cast /castrandom /castsequence /changeactionbar /clearfocus /cleartarget /click /dismount /equip /equipslot /equipset /focus /petagressive /petattack /petautocastoff /petautocaston /petdefensive /petfollow /petpassive /petstay /startattack /stopattack /stopcasting /stopmacro /swapactionbar /target /targetenemy /targetfriend /targetlasttarget /targetparty /targetraid /use /usetalents /userandom").split(" ");
    var optionKeywords = ("actionbar button channeling combat dead equipped exists flyable flying group harm help indoors modifier mounted outdoors party pet raid stance spec stealth swimming target unithasvehicleui vehicleui").split(" ");
    var emoteKeywords = ("/tired /victory /wave /welcome /whine /whistle /work /yawn /boggle /calm /bleed /blood /cold /comfort /cuddle /spoon /duck /insult /introduce /jk /lick /listen /lost /blink /mock /ponder /pounce /praise /lavish /purr /puzzled /raise /volunteer /ready /rdy /shimmy /shiver /blush /shoo /pest /slap /smirk /sniff /snub /soothe /stink /smell /taunt /tease /thirsty /bonk /doh /veto /snicker /tickle /stand /violin /smile /rasp /growl /bark /pity /bored /scared /flop /love /moo /commend /train /helpme /incoming /openfire /charge /bounce /flee /attacktarget /oom /followme /wait /flirt /healme /silly /wink /pat /brb /golfclap /mountspecial /bow /burp /belch /bye /goodbye /farewell /agree /cackle /cheer /woot /chicken /flap /strut /chuckle /clap /confused /congratulate /congrats /cong /grats /unused /cough /cower /fear /amaze /inc /retreat /crack /knuckles /cringe /cry /sob /weep /curious /curtsey /dance /blame /blank /drink /shindig /brandish /breath /disagree /doubt /embarrass /encourage /enemy /eyebrow /brow /drool /highfive /absent /arm /awe /backpack /pack /badfeeling /bad /challenge /chug /ding /eat /chew /feast /facepalm /palm /faint /go /going /glower /headache /hiccup /hiss /holdhand /eye /angry /mad /hurry /idea /jealous /luck /map /mercy /mutter /nervous /offer /fart /pet /pinch /proud /promise /pulse /punch /pout /regret /fidget /impatient /revenge /rolleyes /eyeroll /ruffle /sad /scoff /scold /scowl /search /shakefist /fist /shifty /flex /strong /shudder /signal /silence /shush /sing /smack /sneak /sneeze /snort /squeal /frown /disappointed /disappointment /suspicious /think /truce /twiddle /warn /snap /charm /coverears /crossarms /look /gasp /object /objection /holdit /sweat /yw /read /gaze /giggle /glare /gloat /greet /greetings /apologize /sorry /grin /wicked /wickedly /groan /grovel /peon /guffaw /hail /happy /glad /yay /hello /hi /hug /hungry /food /pizza /kiss /blow /applaud /bravo /applause /kneel /laugh /lol /laydown /liedown /lay /lie /massage /moan /moon /mourn /no /nod /yes /nosepick /pick /bashful /panic /peer /plead /point /poke /pray /roar /rawr /rofl /rude /salute /beckon /scratch /cat /catty /sexy /shake /rear /shout /holler /shrug /shy /sigh /sit /sleep /snarl /beg /spit /stare /surprised /surrender /talk /talkex /excited /talkq /question /tap /thank /thanks /ty /threaten /doom /threat").split(" ");

    //Class Cast Sequence
    var deathKnightKeywords = ("Heart Strike,Frost Strike,Scourge Strike,Obliterate Off-Hand,Blood Strike,Death Coil,Death Gate,Death Grip,Frost Presence,Icy Touch,Plague Strike,Rune of Cinderglacier,Rune of Razorice,Runeforging,Death Strike,Pestilence,Raise Dead,Blood Presence,Mind Freeze,Rune of Spellbreaking,Rune of Spellshattering,Blood Boil,Chains of Ice,Strangulate,Death and Decay,Rune of Lichbane,Obliterate,Path of Frost,Icebound Fortitude,Rune of Swordbreaking,Rune of Swordshattering,Blood Tap,Festering Strike,Dark Command,Horn of Winter,Death Pact,Rune Strike,Anti-Magic Shell,Rune of the Fallen Crusader,Unholy Presence,Raise Ally,Rune of the Nerubian Carapace,Rune of the Stoneskin Gargoyle,Festering Strike Off-Hand,Empower Rune Weapon,Army of the Dead,Mastery,Outbreak,Necrotic Strike,Dark Simulacrum,Anti-Magic Zone,Bone Shield,Dancing Rune Weapon,Dark Transformation,Howling Blast,Hungering Cold,Improved Icy Talons,Lichborne,Pillar of Frost,Rune Tap,Summon Gargoyle,Unholy Blight,Unholy Frenzy,Vampiric Blood").split(",");
    var druidKeywords = ("Starsurge,Mangle,Swiftmend,Wrath,Moonfire,Thorns,Entangling Roots,Cat Form,Claw,Ferocious Bite,Nourish,Rake,Rejuvenation,Starfire,Mangle,Prowl,Regrowth,Revive,Bear Form,Demoralizing Roar,Growl,Maul,Teleport: Moonglade,Aquatic Form,Travel Form,Swipe,Insect Swarm,Rebirth,Enrage,Ravage,Ravage!,Skull Bash,Faerie Fire,Faerie Fire (Feral),Remove Corruption,Tiger's Fury,Cower,Dash,Challenging Roar,Innervate,Soothe,Mark of the Wild,Track Humanoids,Pounce,Hurricane,Shred,Hibernate,Leather Specialization,Frenzied Regeneration,Nature's Grasp,Rip,Barkskin,Flight Form,Maim,Lifebloom,Lacerate,Tranquility,Swift Flight Form,Cyclone,Savage Roar,Healing Touch,Mastery,Wild Mushroom,Thrash,Stampeding Roar,Wild Mushroom: Detonate,Berserk,Earth and Moon,Feral Charge,Force of Nature,Leader of the Pack,Master Shapeshifter,Moonkin Form,Nature's Cure,Nature's Swiftness,Pulverize,Solar Beam,Starfall,Sunfire,Survival Instincts,Tree of Life,Typhoon,Wild Growth").split(",");
    var hunterKeywords = ("Intimidation,Aimed Shot,Explosive Shot,Arcane Shot,Auto Shot,Call Pet 1,Revive Pet,Steady Shot,Track Beasts,Raptor Strike,Concussive Shot,Aimed Shot!,Beast Lore,Dismiss Pet,Feed Pet,Kill Command,Serpent Sting,Tame Beast,Aspect of the Hawk,Track Humanoids,Wing Clip,Disengage,Hunter's Mark,Scatter Shot,Aspect of the Cheetah,Eagle Eye,Mend Pet,Call Pet 2,Track Undead,Immolation Trap,Multi-Shot,Track Hidden,Freezing Trap,Feign Death,Track Elementals,Kill Shot,Tranquilizing Shot,Scare Beast,Track Demons,Explosive Trap,Flare,Widow Venom,Call Pet 3,Ice Trap,Track Giants,Trap Launcher,Mail Specialization,Distracting Shot,Track Dragonkin,Rapid Fire,Aspect of the Pack,Call Pet 4,Aspect of the Wild,Snake Trap,Master's Call,Misdirection,Deterrence,Mastery,Cobra Shot,Call Pet 5,Aspect of the Fox,Camouflage,Beast Mastery,Bestial Wrath,Black Arrow,Chimera Shot,Counterattack,Fervor,Focus Fire,Hunting Party,Readiness,Sic 'Em!,Silencing Shot,The Beast Within,Trueshot Aura,Wyvern Sting").split(",");
    var mageKeywords = ("Arcane Barrage,Pyroblast,Summon Water Elemental,Fireball,Arcane Missiles,Fire Blast,Frostbolt,Frost Nova,Counterspell,Pyroblast!,Evocation,Polymorph,Blink,Cone of Cold,Arcane Blast,Arcane Explosion,Teleport: Darnassus,Teleport: Exodar,Teleport: Ironforge,Teleport: Orgrimmar,Teleport: Silvermoon,Teleport: Stormwind,Teleport: Theramore,Teleport: Thunder Bluff,Teleport: Undercity,Scorch,Ice Lance,Ice Block,Remove Curse,Slow Fall,Molten Armor,Mage Ward,Conjure Refreshment,Portal: Darnassus,Portal: Exodar,Portal: Ironforge,Portal: Orgrimmar,Portal: Silvermoon,Portal: Stormwind,Portal: Theramore,Portal: Thunder Bluff,Portal: Undercity,Flamestrike,Mana Shield,Conjure Mana Gem,Mirror Image,Blizzard,Portal: Stonard,Teleport: Stonard,Frost Armor,Frostfire Bolt,Arcane Brilliance,Teleport: Shattrath,Portal: Shattrath,Mage Armor,Spellsteal,Teleport: Dalaran,Portal: Dalaran,Ritual of Refreshment,Invisibility,Dalaran Brilliance,Mastery,Flame Orb,Ring of Frost,Portal: Tol Barad,Teleport: Tol Barad,Time Warp,Arcane Power,Arcane Tactics,Blast Wave,Cold Snap,Combustion,Deep Freeze,Deep Freeze,Dragon's Breath,Fire Power,Focus Magic,Ice Barrier,Icy Veins,Living Bomb,Presence of Mind,Slow").split(",");
    var paladinKeywords = ("Holy Shock,Avenger's Shield,Templar's Verdict,Crusader Strike,Judgement,Seal of Righteousness,Devotion Aura,Holy Light,Word of Glory,Redemption,Righteous Fury,Hammer of Justice,Hand of Reckoning,Flash of Light,Lay on Hands,Exorcism,Hand of Protection,Blessing of Kings,Consecration,Retribution Aura,Holy Wrath,Divine Protection,Seal of Insight,Cleanse,Righteous Defense,Concentration Aura,Divine Plea,Seal of Truth,Hammer of Wrath,Divine Shield,Plate Specialization,Hand of Freedom,Rebuke,Blessing of Might,Crusader Aura,Divine Light,Seal of Justice,Hand of Salvation,Avenging Wrath,Resistance Aura,Turn Evil,Hand of Sacrifice,Mastery,Inquisition,Holy Radiance,Guardian of Ancient Kings,Ardent Defender,Aura Mastery,Beacon of Light,Communion,Divine Favor,Divine Guardian,Divine Storm,Hammer of the Righteous,Holy Shield,Light of Dawn,Repentance,Sacred Shield,Seals of Command,Shield of the Righteous,Vindication,Zealotry").split(",");
    var priestKeywords = ("Penance,Holy Word: Chastise,Mind Flay,Holy Word: Sanctuary,Smite,Flash Heal,Shadow Word: Pain,Power Word: Shield,Inner Fire,Renew,Mind Blast,Psychic Scream,Power Word: Fortitude,Resurrection,Heal,Holy Fire,Blessed Healing,Holy Word: Serenity,Cure Disease,Fade,Dispel Magic,Devouring Plague,Shackle Undead,Shadow Word: Death,Levitate,Mind Vision,Greater Heal,Mind Control,Prayer of Healing,Binding Heal,Shadow Protection,Fear Ward,Mind Soothe,Mana Burn,Holy Nova,Hymn of Hope,Shadowfiend,Prayer of Mending,Mass Dispel,Mind Sear,Divine Hymn,Mastery,Mind Spike,Inner Will,Leap of Faith,Archangel,Chakra,Circle of Healing,Desperate Prayer,Dispersion,Divine Touch,Guardian Spirit,Inner Focus,Lightwell,Pain Suppression,Power Infusion,Power Word: Barrier,Psychic Horror,Shadowform,Silence,Spirit of Redemption,Vampiric Embrace,Vampiric Touch").split(",");
    var rogueKeywords = ("Mutilate,Blade Flurry,Shadowstep,Sinister Strike,Eviscerate,Sinister Strike Enabler,Stealth,Pick Pocket,Ambush,Evasion,Sap,Recuperate,Kick,Gouge,Pick Lock,Sprint,Backstab,Slice and Dice,Vanish,Cheap Shot,Distract,Kidney Shot,Blind,Expose Armor,Dismantle,Garrote,Feint,Disarm Trap,Rupture,Leather Specialization,Envenom,Cloak of Shadows,Deadly Throw,Shiv,Tricks of the Trade,Fan of Knives,Mastery,Combat Readiness,Redirect,Smoke Bomb,Adrenaline Rush,Cold Blood,Hemorrhage,Killing Spree,Master Poisoner,Overkill,Premeditation,Preparation,Revealing Strike,Shadow Dance,Vendetta").split(",");
    var shamanKeywords = ("Thunderstorm,Lava Lash,Earth Shield,Unleash Flame,Unleash Frost,Unleash Wind,Lightning Bolt,Primal Strike,Strength of Earth Totem,Earth Shock,Healing Wave,Lightning Shield,Flametongue Weapon,Searing Totem,Ancestral Spirit,Fire Nova,Flametongue Totem,Purge,Flame Shock,Ghost Wolf,Wind Shear,Cleanse Spirit,Earthbind Totem,Healing Stream Totem,Healing Surge,Water Shield,Frost Shock,Water Walking,Frostbrand Weapon,Chain Lightning,Astral Recall,Call of the Elements,Totemic Recall,Windfury Totem,Windfury Weapon,Lava Burst,Far Sight,Magma Totem,Grounding Totem,Call of the Ancestors,Chain Heal,Mana Spring Totem,Wrath of Air Totem,Water Breathing,Stoneskin Totem,Call of the Spirits,Mail Specialization,Tremor Totem,Earthliving Weapon,Earth Elemental Totem,Stoneclaw Totem,Elemental Resistance Totem,Bind Elemental,Fire Elemental Totem,Greater Healing Wave,Bloodlust,Heroism,Totem of Tranquil Mind,Rockbiter Weapon,Hex,Mastery,Unleash Elements,Healing Rain,Spiritwalker's Grace,Earthquake,Elemental Focus,Elemental Mastery,Feral Spirit,Fulmination,Improved Cleanse Spirit,Mana Tide Totem,Nature's Swiftness,Riptide,Shamanistic Rage,Spirit Link Totem,Stormstrike,Totemic Wrath").split(",");
    var warlockKeywords = ("Unstable Affliction,Summon Felguard,Conflagrate,Demon Leap,Shadow Bolt,Soul Swap Exhale,Summon Imp,Immolate,Corruption,Life Tap,Drain Life,Demon Armor,Summon Voidwalker,Create Healthstone,Drain Soul,Soulburn,Bane of Agony,Health Funnel,Soul Harvest,Fear,Curse of Weakness,Unending Breath,Create Soulstone,Rain of Fire,Searing Pain,Bane of Doom,Soul Fire,Soul Link,Summon Succubus,Eye of Kilrogg,Curse of Tongues,Enslave Demon,Hellfire,Summon Felhunter,Banish,Shadow Ward,Death Coil,Ritual of Summoning,Howl of Terror,Infernal Awakening,Summon Infernal,Curse of the Elements,Summon Doomguard,Demon Leap,Immolation,Immolation Aura,Fel Armor,Incinerate,Soulshatter,Ritual of Souls,Seed of Corruption,Shadowflame,Demonic Circle: Summon,Demonic Circle: Teleport,Mastery,Fel Flame,Dark Intent,Demon Soul,Bane of Havoc,Chaos Bolt,Curse of Exhaustion,Demonic Empowerment,Hand of Gul'dan,Haunt,Inferno,Metamorphosis,Nether Ward,Nether Ward,Shadowburn,Shadowfury,Soul Swap,Soulburn: Seed of Corruption").split(",");
    var warriorKeywords = ("Mortal Strike,Bloodthirst,Shield Slam,Attack,Battle Stance,Strike,Charge,Victory Rush,Rend,Thunder Clap,Defensive Stance,Taunt,Heroic Strike,Execute,Sunder Armor,Battle Shout,Heroic Throw,Overpower,Cleave,Hamstring,Shield Block,Berserker Stance,Disarm,Whirlwind,Pummel,Revenge,Intimidating Shout,Slam,Challenging Shout,Shield Wall,Intercept,Plate Specialization,Demoralizing Shout,Berserker Rage,Inner Rage,Retaliation,Recklessness,Spell Reflection,Commanding Shout,Intervene,Shattering Throw,Enraged Regeneration,Mastery,Colossus Smash,Rallying Cry,Heroic Leap,Bladestorm,Concussion Blow,Deadly Calm,Death Wish,Devastate,Heroic Fury,Last Stand,Piercing Howl,Raging Blow,Shockwave,Single-Minded Fury,Sweeping Strikes,Throwdown,Vigilance,Warbringer").split(",");

    //Race Cast Sequence
    var bloodElfKeywords = ("Arcane Torrent").split(",");
    var draeneiKeywords = ("Gift of the Naaru").split(",");
    var dwarfKeywords = ("Stoneform").split(",");
    var gnomeKeywords = ("Escape Artist").split(",");
    var goblinKeywords = ("Pack Hobgoblin,Rocket Barrage,Rocket Jump").split(",");
    var humanKeywords = ("Every Man for Himself").split(",");
    var nightElfKeywords = ("Shadowmeld").split(",");
    var orcKeywords = ("Blood Fury").split(",");
    var taurenKeywords = ("War Stomp").split(",");
    var trollKeywords = ("Berserking").split(",");
    var undeadKeywords = ("Cannibalize,Will of the Forsaken").split(",");
    var worgenKeywords = ("Darkflight,Running Wild,Two Forms").split(",");

    function getCompletions(token, context) {
        var found = [], start = token.string;
        var items = start.replace("[", "").replace("]", "").split(",");

        start = items[items.length - 1];
        function maybeAdd(str) {
            if (str.indexOf(start) == 0 || start.indexOf(" ") == 0) found.push(str);
        }

        function add(str) {
            found.push(str);
        }

        function gatherCastCompletions(forceAdd) {
            var playerClass = $("#classSelect").val();
            var classKeywords;
            var raceKeywords;

            if (playerClass.indexOf("DeathKnight") == 0)
                classKeywords = deathKnightKeywords;
            else if (playerClass.indexOf("Durid") == 0)
                classKeywords = druidKeywords;
            else if (playerClass.indexOf("Hunter") == 0)
                classKeywords = hunterKeywords;
            else if (playerClass.indexOf("Mage") == 0)
                classKeywords = mageKeywords;
            else if (playerClass.indexOf("Paladin") == 0)
                classKeywords = paladinKeywords;
            else if (playerClass.indexOf("Priest") == 0)
                classKeywords = priestKeywords;
            else if (playerClass.indexOf("Rogue") == 0)
                classKeywords = rogueKeywords;
            else if (playerClass.indexOf("Shaman") == 0)
                classKeywords = shamanKeywords;
            else if (playerClass.indexOf("Warlock") == 0)
                classKeywords = warlockKeywords;
            else if (playerClass.indexOf("Warrior") == 0)
                classKeywords = warriorKeywords;

            if(forceAdd)
                forEach(classKeywords, add);
            else
                forEach(classKeywords, maybeAdd);

            var playerRace = $("#raceSelect").val();
            if (playerRace.indexOf("BloodElf") == 0)
                raceKeywords = bloodElfKeywords;
            else if (playerRace.indexOf("Draenei") == 0)
                raceKeywords = draeneiKeywords;
            else if (playerRace.indexOf("Dwarf") == 0)
                raceKeywords = dwarfKeywords;
            else if (playerRace.indexOf("Gnome") == 0)
                raceKeywords = gnomeKeywords;
            else if (playerRace.indexOf("Goblin") == 0)
                raceKeywords = goblinKeywords;
            else if (playerRace.indexOf("Human") == 0)
                raceKeywords = humanKeywords;
            else if (playerRace.indexOf("NightElf") == 0)
                raceKeywords = nightElfKeywords;
            else if (playerRace.indexOf("Orc") == 0)
                raceKeywords = orcKeywords;
            else if (playerRace.indexOf("Tauren") == 0)
                raceKeywords = taurenKeywords;
            else if (playerRace.indexOf("Troll") == 0)
                raceKeywords = trollKeywords;
            else if (playerRace.indexOf("Undead") == 0)
                raceKeywords = undeadKeywords;
            else if (playerRace.indexOf("Worgen") == 0)
                raceKeywords = worgenKeywords;

            if (forceAdd)
                forEach(raceKeywords, add);
            else
                forEach(raceKeywords, maybeAdd);
        }


        function gatherCompletions(obj) {
            if (obj == "meta") forEach(PoundKeywords, maybeAdd);
            else if (obj == "slash") {
                forEach(slashKeywords, maybeAdd);
                forEach(emoteKeywords, maybeAdd);
            }
            else if (obj == "bracket")
                forEach(optionKeywords, maybeAdd);
            else if (obj == "cast") {
                gatherCastCompletions(false);
            }
            else if (obj == "castAdd") {
                gatherCastCompletions(true);
            }
            else if (obj == "all") {
                forEach(PoundKeywords, maybeAdd);
                forEach(slashKeywords, maybeAdd);
                forEach(emoteKeywords, maybeAdd);
            }
        }

        if (context) {
            // If this is a property, see if it belongs to some object we can
            // find in the current environment.
            var obj = context.pop(), base;
            if (obj.className == "meta-command")
                base = "meta";
            else if (obj.className == "slash-command")
                base = "slash";
            else if (obj.className == "bracket-command")
                base = "bracket";
            else if (obj.className == "cast-ability")
                base = "cast";
            while (base != null && context.length)
                base = base[context.pop().string];
            if (base != null) gatherCompletions(base);
        }
        else {
            if (editor.getLine(editor.getCursor(false).line).indexOf("/cast") === 0)
                gatherCompletions("castAdd");
            else
                gatherCompletions("all");

        }
        return jQuery.unique(found).sort();
    }
})();


