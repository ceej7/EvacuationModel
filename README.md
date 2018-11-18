# EvacuationModel
A evacuation model in the subway scene
## Env
Unity, VRTK
## Evacuation Model
A two-layer model design for human flow evacuation, macro and micro parts. 
- Marco one divides the whole diagram into several blocks, using time-spreading dynamic network flow to deal with the collaboration among blocks.
- Micro one cares more about individual behaviours. Combining the Floyd algorithm, Load Balance Functionand Probability Density Distribution Model, it ideally simulates personal actions.
## Virtual Reality
In addition to the model, we implemented the first-person perspective to allow the player to participate in the evacuation process.
## Demo
![image](https://github.com/ceej7/EvacuationModel/blob/master/Demo/1.jpg)

