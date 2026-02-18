import './ReadingChallengesPage.css';

import { MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { useChallengePage } from '@/features/challenges/hooks/useChallengePage';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import { ErrorRedirect } from '@/shared/components/errors/redirect/ErrorsRedirect';
import GoalCard from '../goal-card/GoalCard';
import StreakCard from '../streak-card/StreakCard';
import type { HttpError } from '@/shared/types/errors/httpError';

const ReadingChallengesPage: FC = () => {
  const { t } = useTranslation('challenges');
  const {
    year,
    goalType,
    setGoalType,
    goalValue,
    setGoalValue,
    unitLabel,
    progress,
    streak,
    isFetching,
    isSaving,
    isCheckingIn,
    error,
    saveGoal,
    checkInTodayHandler,
  } = useChallengePage();

  if (error) {
    return <ErrorRedirect error={error as HttpError} />;
  }

  return (
    <div className="challenge-page">
      <MDBContainer className="challenge-page__container my-5">
        <div className="challenge-page__header">
          <h2 className="challenge-page__title">{t('page.title')}</h2>
          <p className="challenge-page__subtitle">{t('page.subtitle')}</p>
        </div>

        {isFetching && (
          <div className="challenge-page__spinner">
            <DefaultSpinner />
          </div>
        )}

        {!isFetching && (
          <MDBRow className="g-4">
            <MDBCol lg="7">
              <GoalCard
                year={year}
                progress={progress}
                goalType={goalType}
                setGoalType={setGoalType}
                goalValue={goalValue}
                setGoalValue={setGoalValue}
                unitLabel={unitLabel}
                isFetching={isFetching}
                isSaving={isSaving}
                onSave={saveGoal}
              />
            </MDBCol>
            <MDBCol lg="5">
              <StreakCard
                streak={streak}
                isFetching={isFetching}
                isCheckingIn={isCheckingIn}
                onCheckIn={checkInTodayHandler}
              />
            </MDBCol>
          </MDBRow>
        )}
      </MDBContainer>
    </div>
  );
};

export default ReadingChallengesPage;
