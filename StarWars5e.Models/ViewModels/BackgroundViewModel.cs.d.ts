declare module server {
	interface backgroundViewModel extends tableEntity {
		proficiencyOptions: string;
		toolOptions: string;
		languagesLearned: string;
		specialityOptions: string;
		featOptions: string;
		personalityTraits: string;
		ideals: string;
		bonds: string;
		flaws: string;
		/** The name of the background */
		name: string;
		/** The description of the background */
		description: string;
		/** The number of proficiencies that you get to choose */
		proficiencyCount: number;
		/** The flavor text that contains the proficiency list */
		proficiencyMarkdown: string;
		/** The flavor text that contains the tool proficiency list */
		toolProficiencyMarkdown: string;
		/** The number of languages that this background gets to learn */
		languages: number;
		/** The flavor text for languages this background gets */
		languageMarkdown: string;
		/** The flavor text associated with the equipment options this bakcground has */
		equipmentMarkdown: string;
		/** The name of the (optional) speciality of this background */
		specialityName: string;
		/** The flavor text for this background speciality */
		specialityMarkdown: string;
		/** The name of the special feature this background has */
		featureName: string;
		/** The flavor text that this background feature has */
		featureMarkdown: string;
		/** The flavor text for the background feat */
		backgroundFeatMarkdown: string;
		/** The flavor text for the suggested characteristics of this background */
		suggestedCharacteristicsMarkdown: string;
	}
}
