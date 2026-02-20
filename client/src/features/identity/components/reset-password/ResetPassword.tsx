import './ResetPassword.css';

import type { FC } from 'react';
import React from 'react';
import { useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'react-router-dom';

import { useResetPassword } from '@/features/identity/hooks/useIdentity';
import { IsError } from '@/shared/lib/utils/utils';

const ResetPassword: FC = () => {
  const { t } = useTranslation('identity');
  const resetPassword = useResetPassword();
  const [searchParams] = useSearchParams();

  const email = useMemo(() => searchParams.get('email') ?? '', [searchParams]);
  const token = useMemo(() => searchParams.get('token') ?? '', [searchParams]);

  const [newPassword, setNewPassword] = useState('');
  const [confirm, setConfirm] = useState('');
  const [error, setError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!email || !token) {
      setError(t('resetPassword.validation.missingToken'));
      return;
    }
    if (!newPassword.trim()) {
      setError(t('resetPassword.validation.passwordRequired'));
      return;
    }
    if (!confirm.trim()) {
      setError(t('resetPassword.validation.confirmRequired'));
      return;
    }
    if (newPassword !== confirm) {
      setError(t('resetPassword.validation.passwordsMustMatch'));
      return;
    }

    setIsSubmitting(true);
    try {
      await resetPassword({ email, token, newPassword });

      setNewPassword('');
      setConfirm('');
    } catch (error) {
      setError(IsError(error) ? error.message : t('messages.unknownError'));
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="identity-auth reset-password-page">
      <div className="login-form-card">
        <h2 className="login-title">{t('resetPassword.title')}</h2>
        <p className="auth-hint">{t('resetPassword.hint')}</p>

        {error && <div className="auth-form-error">{error}</div>}

        <form onSubmit={onSubmit} noValidate>
          <div className="auth-field">
            <label htmlFor="email" className="auth-label">
              {t('resetPassword.labels.email')}
            </label>
            <input id="email" className="auth-input" value={email} disabled />
          </div>

          <div className="auth-field">
            <label htmlFor="newPassword" className="auth-label">
              {t('resetPassword.labels.newPassword')}
            </label>
            <input
              id="newPassword"
              type="password"
              className="auth-input"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
              autoComplete="new-password"
            />
          </div>

          <div className="auth-field">
            <label htmlFor="confirm" className="auth-label">
              {t('resetPassword.labels.confirm')}
            </label>
            <input
              id="confirm"
              type="password"
              className="auth-input"
              value={confirm}
              onChange={(e) => setConfirm(e.target.value)}
              required
              autoComplete="new-password"
            />
          </div>

          <button
            className="auth-submit-btn"
            type="submit"
            disabled={isSubmitting}
            aria-busy={isSubmitting}
          >
            {isSubmitting
              ? t('resetPassword.buttons.submitting')
              : t('resetPassword.buttons.submit')}
          </button>
        </form>
      </div>
    </div>
  );
};

export default ResetPassword;
