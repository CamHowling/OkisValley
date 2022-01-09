/*
 * 
 *  TODO 
 *  
 *  Create/Add HexTile /
 *  Create/Source Basic materials: Dirt, Water, Field, Town, Rock, Resources /
 *      
 *      
 *      
 *  Create/Source avatar for Oki /
 *  Import and adapt raycasting interaction and movement /
 *  Import User interface and fix bug / 
 *  Build terrain out /
 *  Add non interactable mountain region /
 *  Create/Source audio feedback cues: Cannot Place, Hit Movement Boundary, Replacement successful, UI 'swap'
 *  Create/Source visual feedback cues: Extend UI to grow/glow as it clicks into place, particle effect when ground is replaced, particle effect for colliding with movement boundary
 *  
 *      Ground, Dirt, Pasture, Water, Lake, Iron, Gold, Diamond, Farm
 *  
 *  Create/Source tile tokens: Animals, Buildings, and so on
 *      
 *      Tokens: Human ( y = 0.4, scale 0.2) , Cow/Goat, Pickaxe (rotate)
 *  
 *  Update UI Images /
 *  
 *  Develop stats / (Partially done, needs some trouble shooting)
 * 
 * 
 * Requirements:
 * 1) Scripts that provide control and interaction to world:
 *      Movement of Oki, Raycasting to change tile based on selection /
 * 2) Demonstrate a sequence of steps and management of states
 *      Heads up display for selection of ground types / 
 * 3) Demonstrate modifying the environment in a meaningful way
 *      Changing the material of a given tile / ----- (Each ground tile will run calculations) / (needs minor updates)
 * 4) Feedback (Visual/Audio)
 *      UI, Movement boundary, and ground placement all provide feedback    (Tile placement feedback added, but not working, have several sounds, and UI buttons change alpha) /
 * 5) Application parameters available via the User interface, and can be manipulated by modifying the world
 *      UI will present Worship, productivty, and happiness as elements that can be manipulated by sculping the world correctly / partially done, have to add calculations for other things /
 * 6) Elements of the environment need to provide autonomous simulation, involving multiple objects effecting each other
 *      Hex tiles will provide real time updates to worship, productivity, and happiness by checking nearby tiles for type and tokens /
 *      For example when a town tile is near a field tile, animals can spawn, if animals are nearby a town tile, happiness will increase (remaining)
 * 7) Application needs a VR start screen that describes title, author details, the controls, and purpose. / Mostly Done - needs to be cleaned up a little
 *    Application must be able to return to this screen to restart experience. 
 * 
 */