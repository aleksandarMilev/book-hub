import './GoalCard.css';

import {
  MDBBadge,
  MDBCard,
  MDBCardBody,
  MDBCardText,
  MDBCardTitle,
  MDBInput,
  MDBProgress,
  MDBProgressBar,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import {
  type ReadingChallengeProgress,
  ReadingGoalType,
} from '@/features/challenges/types/challenge';

type Props = {
  year: number;
  progress: ReadingChallengeProgress | null;
  goalType: ReadingGoalType;
  setGoalType: (goalType: ReadingGoalType) => void;
  goalValue: string;
  setGoalValue: (goalValue: string) => void;
  unitLabel: string;
  isFetching: boolean;
  isSaving: boolean;
  onSave: () => void;
};

const GoalCard: FC<Props> = ({
  year,
  progress,
  goalType,
  setGoalType,
  goalValue,
  setGoalValue,
  isFetching,
  isSaving,
  onSave,
}) => {
  const { t } = useTranslation('challenges');

  const currentValue = progress?.currentValue ?? 0;
  const percent = progress?.progressPercent ?? 0;

  const unitKey = goalType === ReadingGoalType.Pages ? 'pages' : 'books';

  const title = t('goalCard.title', { year });
  const subtitle = progress
    ? t('goalCard.subtitle.withProgress', {
        current: currentValue,
        goal: progress.goalValue,
        unitLabel: t(`units.${unitKey}`, { count: progress.goalValue }),
      })
    : t('goalCard.subtitle.noProgress', { year });

  const numericGoalValue = Number(goalValue);
  const isDisabled = isFetching || isSaving || numericGoalValue < 1;

  return (
    <MDBCard className="challenge-goal-card">
      <MDBCardBody className="challenge-goal-card__body">
        <div className="challenge-goal-card__header">
          <div>
            <MDBCardTitle className="challenge-goal-card__title">{title}</MDBCardTitle>
            <MDBCardText className="challenge-goal-card__subtitle">{subtitle}</MDBCardText>
          </div>
          <MDBBadge className="challenge-goal-card__badge" pill>
            {Math.round(percent)}%
          </MDBBadge>
        </div>

        <div className="challenge-goal-card__progress">
          <MDBProgress height="12" className="challenge-goal-card__progress-bar">
            <MDBProgressBar
              width={Math.max(0, Math.min(100, percent))}
              valuemin={0}
              valuemax={100}
            />
          </MDBProgress>

          <div className="challenge-goal-card__progress-meta">
            <span className="challenge-goal-card__progress-left">
              {t('goalCard.progress.current')}: <strong>{currentValue}</strong>
            </span>
            <span className="challenge-goal-card__progress-right">
              {t('goalCard.progress.goal')}: <strong>{progress?.goalValue ?? goalValue}</strong>
            </span>
          </div>
        </div>

        <div className="challenge-goal-card__form">
          <div className="challenge-goal-card__row">
            <label className="challenge-goal-card__label" htmlFor="challenge-goal-type">
              {t('goalCard.form.goalTypeLabel')}
            </label>
            <select
              id="challenge-goal-type"
              className="form-select challenge-goal-card__select"
              value={goalType}
              onChange={(e) => setGoalType(Number(e.target.value) as ReadingGoalType)}
              disabled={isFetching || isSaving}
            >
              <option value={ReadingGoalType.Books}>{t('goalCard.form.types.books')}</option>
              <option value={ReadingGoalType.Pages}>{t('goalCard.form.types.pages')}</option>
            </select>
          </div>

          <div className="challenge-goal-card__row">
            <label className="challenge-goal-card__label" htmlFor="challenge-goal-value">
              {t('goalCard.form.goalValueLabel')}
            </label>
            <MDBInput
              id="challenge-goal-value"
              type="number"
              min={1}
              step={1}
              className="challenge-goal-card__input"
              value={goalValue}
              onChange={(event) => setGoalValue(event.target.value)}
              disabled={isFetching || isSaving}
            />
            <div className="challenge-goal-card__hint">{t('goalCard.form.hint')}</div>
          </div>

          <div className="challenge-goal-card__actions">
            <button
              type="button"
              className="btn challenge-goal-card__save-btn"
              disabled={isDisabled}
              onClick={onSave}
            >
              {isSaving ? t('goalCard.actions.saving') : t('goalCard.actions.save')}
            </button>
          </div>
        </div>
      </MDBCardBody>
    </MDBCard>
  );
};

export default GoalCard;
