declare module server {
	interface speciesViewModel {
		/** Name of the species */
		name: string;
		/** Images that are registered for this species */
		imageUrls: string;
		/** Valid colors of the skin */
		skinColors: string;
		/** Valid colors of the species hair */
		hairColors: string;
		/** Valid colors of the species eyes */
		eyeColors: string;
		/** Distinctive Features of the species */
		distinctions: string;
		/** The low end of height for the species */
		mininumHeight: string;
		/** The Modifier for the height of the species */
		heightModifier: string;
		/** The low end of the height for the species */
		minimumWeight: string;
		/** The modifier that is applied to the weight */
		weightModifier: string;
		/** The name of the species homeworld */
		homeworld: string;
		/** The languages the species (generally) knows by default */
		defaultLanguages: string;
		/** Companies that make this droid type */
		manufacturers: string;
		/** Valid droid color schemes */
		colorSchemes: string;
		/** The biology and appearance description of the species */
		biologyMarkdown: string;
		/** Name of the Biology section differs (based on droids etc) */
		biologyName: string;
		/** The society and culture markdown of the species */
		societyMarkdown: string;
		/** The short description that is displayed before the common names */
		nameMarkdown: string;
		/** Common female names of the species */
		femaleNames: string;
		/** Common male names of the species */
		maleNames: string;
		/** Common surnames for the species */
		surnames: string;
		/** Common age milestones for the species */
		age: string;
		/** Alignment tendencies for the species */
		alignment: string;
		/** The size for the species */
		size: any;
		/** The size description of the species */
		sizeMarkdown: string;
		/** The speed description of the species */
		speedMarkdown: string;
		/** Other attribute KvPair for the species */
		otherAttributes: string;
		/** The JSON representation of all statistic increases this species gets (used for tagging) */
		statisticIncreases: string;
		/** Other proficiencies that the species gets (duplicated from other attributes etc) */
		proficienciesGained: string;
		/** The value for statistic increases that will be displayed in the text */
		statisticIncreaseMarkdown: string;
		/** Indicates that this species has darkvision */
		darkvision: boolean;
		/** Markdown for the utility section of droids */
		utilityMarkdown: string;
		/** The name of the note that is provided to GMs */
		gmNoteName: string;
		/** The contents of the note to the GM */
		gmNote: string;
		/** List of common first names for the species */
		firstNames: string;
		/** List of common first names for the species */
		names: string;
		/** List of common nicknames for the species */
		nicknames: string;
	}
}
