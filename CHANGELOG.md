# Changelog

All notable changes to this project will be documented in this file.

## V2 (15/04/2021 - 12PM):
### Added:
- Phrase 3
	- Add in Tobor 3D model without body.
	- Add different facial expression: "Sad" for default fallback answers; "Happy" for feeling happy dialogs; "Normal" for greeting or other dialogs.
- Phrase 4
	- Add in Tobor 3D model with body.
	- Add different facial expression: "Sad" for default fallback answers; "Happy" for feeling happy dialogs; "Normal" for greeting or other dialogs.
- Phrase 5
	- Add in Tobor 3D model with body.
	- Add different facial expression and body animation: "Sad" for default fallback answers; "Happy" for feeling happy dialogs; "Normal" for greeting or other dialogs.
- Phrase 6(AR)
	- Add AR plane detection functions to tap and place the object.
	- Use same animation and dialogs with phrase 5.
	- Text box still using for phrase 6.
- TextToSpeech is added from phrase 3 onwards.
- Add back button for all phrase to go back Intro scene.

## V1.5 (24/06/2020 - 4PM):
### Added:
- Add notification popup prompting the user to close the narration box before they can continue.
- Change trophy color when POI is completed.
### Changed:
- Disabled scoreboard button, changed color to gray.
- Removed the line in the popup notification screen.
- Removed the x button for image/ video hotspot (only keep the narration x button).
- Add missing texts for first quiz question.

## V1.4 (23/06/2020 - 6PM):
### Changed:
- Fixed quiz explanation top bar’s color was incorrect.
- Fixed a bug that can only zoom from a certain angle.
- Fixed “missed” notification doesn’t hide.
- Expanded item viewing area.

## V1.3 (23/06/2020 - 2PM):
### Changed:
- Fixed a bug that the item list will disappear if pressed twice.
- Fixed a bug that the item list and the pause button disappear when going back to the POI after finishing it.  
- Fixed a bug that only the last collected item can be rotated.
- Fixed a bug that prevents smooth control of the item (zoom and rotate) - further test required
- Change logic to show collected items in the list (but hide the actual item) if the user comes back to the POI again.
- Fixed a bug that user scrolls the text and rotates the item together.
- Fixed a bug that ripple effect will still show for collected items.
- Fixed a bug that Pause menu shows incorrect completion status.
- Fixed text scroll value doesn't reset when text is updated
- Changed background to gradient for item description page.
- Further optimize files size.