export enum ReadingGoalType {
  Books = 0,
  Pages = 1,
}

export interface ReadingChallenge {
  year: number;
  goalType: ReadingGoalType;
  goalValue: number;
}

export interface ReadingChallengeProgress {
  year: number;
  goalType: ReadingGoalType;
  goalValue: number;
  currentValue: number;
  progressPercent: number;
}

export interface ReadingStreak {
  currentStreak: number;
  longestStreak: number;
  checkedInToday: boolean;
  today: string;
}

export interface UpsertReadingChallengePayload {
  year: number;
  goalType: ReadingGoalType;
  goalValue: number;
}
