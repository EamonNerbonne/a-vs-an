using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    [UseReporter(typeof(DiffReporter))]
    public class MarkupStripperTests {
        readonly RegexTextUtils utils = new RegexTextUtils();

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_AaRiver() {
            Approvals.Verify(utils.StripWikiMarkup(@"'''Aa River''' may refer to:

*[[Aa (France)]], in northern France
*[[Aa (Meuse)]], in North Brabant, Netherlands
*[[Aa of Weerijs]], in North Brabant, Netherlands
*[[Drentsche Aa]], a river in Groningen, Netherlands
*[[Aabach (Greifensee)]], or Ustermer Aa, a river in Switzerland
*[[Aabach (Afte)]], formerly called the Große Aa, a river in Germany
*Aa, a river in Antwerp, Belgium which joins the [[Nete River]]

== Former names ==

*[[Gauja]], a river in Latvia, formerly known as ''Livländische Aa''
*[[Lielupe]], a river in Latvia, formerly known as ''Kurländische Aa''

{{geodis}}

[[cy:Afon Aa]]
[[et:Aa jõgi]]
[[zh:阿河]]"));
        }

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_AfrikaIslam() {
            Approvals.Verify(utils.StripWikiMarkup(@"'''Afrika Islam''' (born Charles Glenn, 1968, [[New York City]]) also known as the ""''Son of [[Afrika Bambaataa|Bambaataa]]''"", is an American [[hip hop production|hip-hop producer]] <ref>{{cite web|url=http://www.thedjlist.com/djs/AFRIKA_ISLAM/ |title=Afrika Islam |publisher=thedjlist.com |accessdate=2009-07-21}}</ref>. 

==Life==
In the 1980s he moved to [[Los Angeles]] where he co-produced [[Ice T]]'s early albums ''[[Rhyme Pays]]'' and ''[[Power (Ice T album)|Power]]''.
In the late 1990s, Afrika Islam joined German [[techno music|techno]] icon [[Westbam]] to form [[Mr. X and Mr. Y]], a techno duo that made commercial techno with [[Electro (music)|Electro]] influences.

== References ==
{{Reflist}}

{{DEFAULTSORT:Islam, Afrika}}
[[Category:1968 births]]
[[Category:Living people]]
[[Category:Hip hop record producers]]


{{US-record-producer-stub}}

[[als:Afrika Islam]]
[[de:Afrika Islam]]
[[fr:Afrika Islam]]
[[no:Afrika Islam]]
[[pl:Afrika Islam]]"));
        }

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_Abatement() {
            Approvals.Verify(utils.StripWikiMarkup(@"{{wiktionarypar|abatement}}
<!--{{Wikisource1911Enc|Abatement}}-->

'''Abatement''' may refer to:

*[[Abatement of debts and legacies]], a common law doctrine of wills
*[[Abatement in pleading]], a legal defence to civil and criminal actions based purely on procedural and technical issues involving the death of parties
*[[Abatement (heraldry)]], a modification of the shield or coat of arms that supposedly can be imposed by authority (in England supposedly by the Court of Chivalry) for misconduct
* [[Bird abatement]], driving or removing undesired birds from an area
* [[Dust abatement]], the process of inhibiting the creation of excess soil dust, a pollutant that contributes to excess levels of particulate matter
*'''Tax Abatement''' (aka [[Tax holiday]]) is used in the field of economic development to encourage businesses to relocate, expand, and more currently to retain facilities in a community

{{disambig}}

[[zh-yue:減少]]"));
        }


        //TODO: support definition lists.
        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_CookIslandsTransport() {
            Approvals.Verify(utils.StripWikiMarkup(@"{{CIA}}

This article lists '''[[transport]] in the [[Cook Islands]]'''.

; Railways:
: 0.1 km ([[Rarotonga Steam Railway]])
; [[Highway]]s:
:* Total: 320 [[kilometre|km]]
:* Paved: 33 km
:* Unpaved: 287 km (2003 est.)
; Ports and [[harbour]]s:
: Avarua, Avatiu
; [[Merchant marine]]:
:* Total: 1 ship (1,000 GRT or over) totalling 2,310 GRT/{{DWT|2,181|metric|first=yes}}
:* Ships by type: Cargo 1 (1999 est.)
; [[Airport]]s:
: 7 (1999 est.)
; Airports - with paved runways:
:* Total: 2
:* 1,524 to 2,437 m: 1 (1999 est.)
; Airports - with unpaved runways:
:* Total: 6
:* 1,524 to 2,437 m: 3
:* 914 to 1,523 m: 3 (1999 est.)

== See also ==
* [[Cook Islands]]

==External links==
* [http://www.adb.org/Documents/Reports/Consultant/39118-COO/39118-02-COO-TACR.pdf Airports and Ports in the Cook Islands]

{{Transport by country}}
{{Oceania in topic|Transport in}}

[[Category:Transport in the Cook Islands| ]]

{{CookIslands-stub}}"));
        }


        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_DominicaTransport() {
            Approvals.Verify(utils.StripWikiMarkup(@"{{Unreferenced|date=December 2009}}
{{CIA}}

'''Railways:'''
<br/>
There are no railways on Dominica

'''Highways:'''
<br/>
There are a total of 780&nbsp;km of highways and roads in Dominica, 393&nbsp;km of which are paved, leaving 387 unpaved, according to a 1996 estimate. In Dominica, driving is on the left-hand side of the road. Some of the roads in more rural area are rough and unpaved, and require a [[four-wheel drive]] vehicles. 

'''Ports, harbours, and Merchant Marines:'''
<br/>
Dominica has three ports of entry: Portsmouth, Roseau, and Anse-de-Mai. Portsmouth and Anse-de-Mai are located in the northern region of the island, and Roseau is in the south. There are no merchant marines in Dominica according to a 1999 estimate.

'''Airports:'''
2 (1999 estimate.)

'''Airports - with paved runways:'''
<br>''total:''
2
<br>''914 to 1,523 m:''
2 (1999 estimate.)

== See also ==

[[Dominica]]

{{Transport by country}}
{{Transportation in North America}}

{{DEFAULTSORT:Transport In Dominica}}
[[Category:Transport in Dominica| ]]

[[bn:ডোমিনিকার পরিবহন ব্যবস্থা]]
[[lt:Dominikos transportas]]"));
        }

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_Disperser() {
            Approvals.Verify(utils.StripWikiMarkup(@"A '''disperser''' is a one-sided [[randomness extractor|extractor]].<ref>Ronen Shaltiel. Recent developments in explicit construction of extractors. P. 7.</ref> Where an extractor requires that every event gets the same [[probability]] under the [[uniform distribution]] and the extracted distribution, only the latter is required for a disperser. So for a disperser, an event <math>A \subseteq \{0,1\}^{m}</math> we have:
<math>Pr_{U_{m}}[A] > 1 - \epsilon</math>

'''Definition (Disperser):''' ''A'' <math>(k, \epsilon)</math>''-disperser is a function''

<math>Dis: \{0,1\}^{n}\times \{0,1\}^{d}\rightarrow \{0,1\}^{m}</math>

''such that for every distribution'' <math>X</math> ''on'' <math>\{0,1\}^{n}</math> ''with'' <math>H_{\infty}(X) \geq k</math> ''the support of the distribution'' <math>Dis(X,U_{d})</math> ''is of size at least'' <math>(1-\epsilon)2^{m}</math>.

==Graph Theory==
An '''(''N'', ''M'', ''D'', ''K'', ''e'')-disperser''' is a [[bipartite]]
[[graph theory|graph]] with ''N'' vertices on the left side, each with degree ''D'', and ''M'' vertices on the right side, such that every [[subset]] of ''K'' vertices on the left side is connected to more than (1&nbsp;&minus;&nbsp;''e'')''M'' vertices on the right.

An [[Extractor (mathematics)|extractor]] is a related type of graph that guarantees an even stronger property; every '''(''N'', ''M'', ''D'', ''K'', ''e'')-extractor''' is also an '''(''N'', ''M'', ''D'', ''K'', ''e'')-disperser'''.

==Other meanings==
A disperser is a high-speed mixing device used to disperse or dissolve pigments and other solids into a liquid.

==See also==
*[[Expander graph]]

==References==
<references/>

[[Category:Graph families]]


{{Combin-stub}}"));
        }

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_FeaturedList() {
            Approvals.Verify(utils.StripWikiMarkup(@"<noinclude>{| style=""float:right; padding:1em; border:1px solid #A3B1BF; background-color:#E6F2FF; margin:0 0 0.5em 1em""
|{{Shortcut|WP:WIAFL|WP:FL?|WP:FLCR}}
{{FLpages}}
|}
A [[WP:Featured lists|featured list]] exemplifies our very best work. It covers a topic that lends itself to list format (see [[WP:List]]) and, in addition to meeting the requirements for all Wikipedia content (particularly [[WP:Naming conventions|naming conventions]], [[WP:Neutral point of view|neutrality]], [[WP:No original research|no original research]], [[WP:Verifiability|verifiability]], [[WP:Citing sources|citations]], [[WP:Reliable sources|reliable sources]], [[WP:Biographies of living persons|living persons]], [[WP:Non-free content criteria|non-free content]] and [[WP:What Wikipedia is not|what Wikipedia is not]]) a featured list has the following attributes:</noinclude>
<ol>
 <li>'''Prose.''' It features professional standards of writing.</li>
 <li>'''Lead.''' It has an engaging [[WP:Lead section|lead]] that introduces the subject and defines the scope and inclusion criteria.</li>
 <li>'''Comprehensiveness.'''<ul>
  <li>(a) It comprehensively covers the defined scope, providing at least all of the major items and, where practical, a complete set of items; where appropriate, it has annotations that provide useful and appropriate information about the items.</li>
  <li>(b) In length and/or topic, it meets all of the requirements for [[WP:SAL|stand-alone lists]]; does not violate the [[WP:Content forking|content-forking guideline]], does not largely duplicate material from another article, and could not reasonably be included as part of a related article.</li>
 </ul></li>
 <li>'''Structure.''' It is easy to navigate and includes, where helpful, [[Help:Section|section]] headings and [[Help:Sorting|table sort]] facilities.</li>
 <li>'''Style.''' It complies with the [[Wikipedia:Manual of Style|Manual of Style]] and its supplementary pages.<ul>
  <li>(a) ''Visual appeal.'' It makes suitable use of text layout, formatting, [[Help:Table|tables]], and [[Wikipedia:Colours|colour]]; and a minimal proportion of items are redlinked. </li>
  <li>(b) ''Media files.'' It has [[Wikipedia:Images|images]] and other media, if appropriate to the topic, that follow Wikipedia's [[Wikipedia:Image use policy|usage policies]], with succinct [[Wikipedia:Captions|captions]]<!--  and [[Wikipedia:Alternative text for images|alternative (alt) text]] if necessary -->. [[Wikipedia:Non-free content|Non-free]] images and other media satisfy the [[Wikipedia:Non-free content criteria|criteria for the inclusion of non-free content]] and [[Wikipedia:Image copyright tags/Non-free content|are labeled accordingly]].</li>
 </ul></li>
 <li>'''Stability.''' It is not the subject of ongoing [[Wikipedia:Edit war|edit wars]] and its content does not change significantly from day to day, except in response to the featured list process.</li>
</ol><noinclude>

==See also==
* Featured criteria:
** [[Wikipedia:Featured article criteria|featured articles]]
** [[Wikipedia:Featured picture criteria|featured pictures]]
** [[Wikipedia:Featured portal criteria|featured portals]]
** [[Wikipedia:Featured sound criteria|featured sounds]]
** [[Wikipedia:Featured topic criteria|featured topics]]

{{List navbox}}

[[Category:Wikipedia featured content|{{PAGENAME}}]]

</noinclude>"));
        }

        [Fact, MethodImpl(MethodImplOptions.NoInlining)]
        public void Wikipage_Glottis() {
            Approvals.Verify(utils.StripWikiMarkup(@"{{Infobox Anatomy |
  Name         = Glottis |
  Latin        = |
  GraySubject  = |
  GrayPage     = |
  Image        = Arytenoid cartilage.png |
  Caption      = Arytenoid cartilage |
  Image2       = Glottis positions.png |
  Caption2     = Glottis positions |
  Precursor    = |
  System       = |
  Artery       = |
  Vein         = |
  Nerve        = |
  Lymph        = |
  MeshName     = Glottis |
  MeshNumber   = A04.329.364 |
Dorlands = four/000045205 |
DorlandsID = Glottis 
}}
The '''glottis''' is defined as the combination of the [[vocal folds]] and the space in between the folds (the [[rima glottidis]]).<ref>{{eMedicineDictionary|Glottis}}</ref>

==Function==
As the vocal folds vibrate, the resulting vibration produces a ""buzzing"" quality to the speech, called '''[[Voice (phonetics)|voice]]''' or '''voicing''' or '''pronunciation'''. 

Sound production involving only the glottis is called ''glottal''. English has a [[voiceless glottal fricative]] spelled ""h"". In many accents of English the [[glottal stop]] (made by pressing the folds together) is used as a variant [[allophone]] of the phoneme {{IPA|/t/}} (and in some dialects, occasionally of {{IPA|/k/}} and {{IPA|/p/}}); in some languages, this sound is a [[phoneme]] of its own.

Skilled players of the Australian [[didgeridoo]] restrict their glottal opening in order to produce the full range of timbres available on the instrument. 
<ref>See ""Acoustics:  The vocal tract and the sound of a didgeridoo"", by Tarnopolsky et al. in Nature 436, 39 (7 July 2005))</ref>

The vibration produced is an essential component of ''voiced'' [[consonant]]s as well as [[vowel]]s. If the vocal folds are drawn apart, air flows between them causing no vibration, as in the production of voiceless consonants. 

The glottis is also important in the [[valsalva maneuver]].

*Voiced consonants include {{IPA|/v/, /z/, /ʒ/, /d͡ʒ/, /ð/, /b/, /d/, /ɡ/, /w/.}}
*Voiceless consonants include {{IPA|/f/, /s/, /ʃ/, /t͡ʃ/, /θ/, /p/, /t/, /k/, /ʍ/, and /h/.}}

==Additional images==
<gallery>
 Image:Illu07 larynx01.jpg|Larynx
 Image:Gray955.png|The entrance to the larynx, viewed from behind.
 Image:Gray1204.png|The entrance to the larynx.
</gallery>

==See also==
*[[Phonation]]

==References==
{{reflist}}

===References of Glottis Simulator===
*de Menezes Lyra R. Glottis simulator. Anesth Analg. 1999 Jun;88(6):1422-3.[http://www.anesthesia-analgesia.org/cgi/reprint/88/6/1424.pdf]

*Smith, N Ty. Simulation in anesthesia: the merits of large simulators versus small simulators. Current Opinion in Anaesthesiology. 13(6):659-665, December 2000.

{{Larynx anatomy}}

[[Category:Phonetics]]
[[Category:Head and neck]]
[[Category:Human voice]]
[[Category:Respiratory system]]


{{respiratory-stub}}

[[ar:مزمار الحنجرة]]
[[ca:Glotis]]
[[de:Glottis]]
[[es:Glotis]]
[[eo:Gloto]]
[[fr:Glotte]]
[[gl:Glote]]
[[ko:성문]]
[[io:Gloto]]
[[it:Glottide]]
[[nl:Glottis]]
[[pl:Głośnia]]
[[pt:Glote]]
[[fi:Äänirako]]
[[sv:Röstspringa]]
[[tl:Tagukan]]
[[zh:声门]]
"));
        }
    }
}
