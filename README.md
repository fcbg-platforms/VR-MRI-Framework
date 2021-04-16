
# VR-MRI-Framework
![Alt text](MRIRoom.jpg?raw=true "Title")
Unity project included a  IRM 3D model of the MRI of Campus Biotech and of the HUG and multiples existing functions to create an experimentation.

# Introduction

In connection with the VR-MRI system, we have developed an open-source framework, allowing you to create, add and modify a multitude of features to create your own virtual reality experience inside MRI.

# Prerequisites

1. Unity 2018.4.2 or latest
2. **IMPORTANT:** The following project depends of *Final IK* plugin. You must be to have it before using this project. [Final IK](https://assetstore.unity.com/packages/tools/animation/final-ik-14290)

# Before build

* _NamedPipe/GO_server_pipe -> component '_communicate_with_pipes_for_noob' -> uncheck b_debug_discard_named_pipes_start_experiment_alone
* _NamedPipe/GO_client_pipe -> component '_communicate_with_pipes_for_noob' -> uncheck b_debug_discard_named_pipes_start_experiment_alone
* _MainExperimentManager -> component '_main_experiment_manager' -> uncheck b_debug_start_experiment_alone

# To do

* Experience file generator with a user interface

# Copyright and license

The codes are released under GNU General Public License.
