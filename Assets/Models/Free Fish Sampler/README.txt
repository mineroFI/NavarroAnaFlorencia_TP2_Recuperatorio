Free Fish Sampler
3 stylised low-poly fish, FBX + GLB + one 1024x1024 PNG texture each. Taken unchanged from the Low-poly Fish Pack, version 1.1.
The full Low-poly Fish Pack (17 fish) is available on our itch.io page.

CONTENTS
 Models/    3 x .fbx + 3 x .glb (one fish each, same models, texture embedded in both)
 Textures/  3 x Tex_<Fish>.png (1024x1024, same image as embedded)
 LICENSE.txt, README.txt, PROVENANCE.txt

FISH
 Name              Tris total   Size W x H x L (studs)  File
 Clownfish         2584         1.04 x 1.13 x 1.60    Models/Fish_Clownfish.fbx
 Goldfish          3396         1.12 x 1.39 x 1.80    Models/Fish_Goldfish.fbx
 ReefShark         2756         1.68 x 2.19 x 3.00    Models/Fish_ReefShark.fbx
 Triangle counts include the optional Outline part (about 500 tris).

SCALE
 1 unit = 1 stud in FBX and GLB; for other engines scale accordingly. Each GLB sits next to its FBX with the same name, the same size and the same axes (glTF +Y up, nose/front toward -Z); it was exported from the FBX in Blender and its bounding box and QA checks were compared against the FBX. Export settings: no extra FBX scaling, global scale 1.0, forward -Z, up Y, nose toward -Z (Blender +Y), belly at Y=0, centred on X/Z, object scales 1.0. The sizes in the table were read back in Blender. The fish are small (1.6 to 3.0 studs long); scale up for other games.

PARTS
 Each fish is split into separate meshes: Body, Eye, Tail, FinL, FinR, usually Fin_Dorsal and Fin_Anal, plus an Outline part. Tail, FinL and FinR have their pivot at the root so you can rotate them. No animation is included. Outline is an inverted-hull copy of the body that draws a dark rim; delete it if you do not want it. Fins and tails are painted to look right when semi-transparent (for example transparency 0.35).

IMPORT TIPS
 Blender: File > Import > FBX (.fbx) or glTF 2.0 (.glb). Leave scale at 1.0. The texture is embedded in each FBX and also supplied in the Textures folder; if a model shows pink or grey, assign the PNG from the Textures folder to the material's Base Color.
 Unity: drag the FBX into Assets. In the Model tab set Scale Factor 1 (tick Convert Units if you want 1 unit = 1 metre). Extract the texture from the Materials tab or assign the PNG from the Textures folder.
 Godot: drop the GLB into the project (easiest; the texture is embedded). The FBX also works with Godot 4's built-in FBX importer. Assign the PNG from the Textures folder if the texture does not appear.
 Roblox Studio: Avatar tab > Import 3D (3D Importer), pick the FBX, set Merge Meshes OFF if you want separate parts, then check the size in the Explorer. We measured every file in Blender and expect 1 unit = 1 stud, but we have NOT yet run Roblox Studio's Import 3D on these files, so the Roblox import result and on-screen scale are untested by us. If a model arrives far too small or large, change its Size in Studio.

KNOWN LIMITS
 - Low poly with painted gloss. Eyes are simple spheres with painted highlights. Mouths are small.
 - Body patterns are projected from the side, so belly and back stripes stretch a little.
 - Nose direction was checked in Blender renders only. If a fish swims backwards in your engine, rotate it 180 degrees.
 - No rig, no animation, no LODs, no collision meshes.

HOW IT WAS MADE
 See PROVENANCE.txt. Meshes were built by Python scripts in Blender; textures were procedurally painted by script in Blender.

Questions: use the comments on the itch.io page.
