import './StreakCard.css';

import { MDBCard, MDBCardBody, MDBCardText, MDBCardTitle } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaFire, FaCheckCircle } from 'react-icons/fa';

import type { ReadingStreak } from '@/features/challenges/types/challenge';

type Props = {
  streak: ReadingStreak | null;
  isFetching: boolean;
  isCheckingIn: boolean;
  onCheckIn: () => void;
};

const StreakCard: FC<Props> = ({ streak, isFetching, isCheckingIn, onCheckIn }) => {
  const { t } = useTranslation('challenges');

  const checkedInToday = streak?.checkedInToday ?? false;
  const current = streak?.currentStreak ?? 0;
  const longest = streak?.longestStreak ?? 0;

  const buttonLabel = checkedInToday
    ? t('streakCard.actions.checkedIn')
    : isCheckingIn
      ? t('streakCard.actions.checkingIn')
      : t('streakCard.actions.checkIn');

  const disabled = isFetching || isCheckingIn || checkedInToday;

  return (
    <MDBCard className="challenge-streak-card">
      <MDBCardBody className="challenge-streak-card__body">
        <div className="challenge-streak-card__header">
          <MDBCardTitle className="challenge-streak-card__title">
            <FaFire className="challenge-streak-card__icon" /> {t('streakCard.title')}
          </MDBCardTitle>
          <MDBCardText className="challenge-streak-card__subtitle">
            {t('streakCard.subtitle')}
          </MDBCardText>
        </div>

        <div className="challenge-streak-card__stats">
          <div className="challenge-streak-card__stat">
            <div className="challenge-streak-card__stat-value">{current}</div>
            <div className="challenge-streak-card__stat-label">{t('streakCard.stats.current')}</div>
          </div>
          <div className="challenge-streak-card__divider" />
          <div className="challenge-streak-card__stat">
            <div className="challenge-streak-card__stat-value">{longest}</div>
            <div className="challenge-streak-card__stat-label">{t('streakCard.stats.longest')}</div>
          </div>
        </div>

        <div className="challenge-streak-card__actions">
          <button
            type="button"
            className="btn challenge-streak-card__btn"
            disabled={disabled}
            onClick={onCheckIn}
          >
            <FaCheckCircle /> {buttonLabel}
          </button>

          {checkedInToday && (
            <div className="challenge-streak-card__status">{t('streakCard.status.checkedIn')}</div>
          )}
        </div>
      </MDBCardBody>
    </MDBCard>
  );
};

export default StreakCard;
