This is a base template for Osmo based games.

Main points of the architecture
- Supports Standalone, Editor and Osmo build modes
- Uses Factory for dependency injection
- View/Controller and entirely interfaces based system
- Testable

Usage

- In github, use this project as a template and create a repo for your project. Project link: https://github.com/jeevan-byjus/GamepodTemplate
- Create a Unity project and clone your created project inside Assets folder
- Rename the namespace in code from Byjus.Gamepod.Template to Byjus.Gamepod.<your_game_name>
- Change assembly definition names (Scripts and Tests) to game specific name
- Also in Files, open both Osmo and Standalone JSONs and change the name to game specific name.

- For vision, look at Verticals/VisionService and Verticals/InputParser.cs
- Use the interface IExtInputListener to define what external input does your game take

******

- Keep only view related code in Views
- For logic, create a related controller in Controllers. Controllers should have a Init method which is like the start point for controller

******

- Use hierarchy manager to setup connection between Views and Controllers

******

- Finally, interface whatever is external or whatever is a service - SoundManager, AnimationManager, Network, FileSave, Progress anything...
- Use factory to get the services' references in code
