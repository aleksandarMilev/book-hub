import './ForgotPassword.css';

import type { FC } from 'react';
import React from 'react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { useForgotPassword } from '@/features/identity/hooks/useIdentity';
import { IsError } from '@/shared/lib/utils/utils';

const emailRegex = /^\S+@\S+\.\S+$/;

const ForgotPassword: FC = () => {
  const { t } = useTranslation('identity');
  const forgotPassword = useForgotPassword();

  const [email, setEmail] = useState('');
  const [error, setError] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const onSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError('');

    const trimmed = email.trim();
    if (!trimmed) {
      setError(t('forgotPassword.validation.emailRequired'));
      return;
    }
    if (!emailRegex.test(trimmed)) {
      setError(t('forgotPassword.validation.emailInvalid'));
      return;
    }

    setIsSubmitting(true);

    try {
      await forgotPassword(trimmed);
      setEmail('');
    } catch (error) {
      setError(IsError(error) ? error.message : t('messages.unknownError'));
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="identity-auth forgot-password-page">
      <div className="login-form-card">
        <h2 className="login-title">{t('forgotPassword.title')}</h2>
        <p className="auth-hint">{t('forgotPassword.hint')}</p>

        {error && <div className="auth-form-error">{error}</div>}

        <form onSubmit={onSubmit} noValidate>
          <div className="auth-field">
            <label htmlFor="email" className="auth-label">
              {t('forgotPassword.labels.email')}
            </label>
            <input
              id="email"
              type="email"
              className="auth-input"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder={t('forgotPassword.placeholders.email')}
              required
              autoComplete="email"
            />
          </div>

          <button
            className="auth-submit-btn"
            type="submit"
            disabled={isSubmitting}
            aria-busy={isSubmitting}
          >
            {isSubmitting
              ? t('forgotPassword.buttons.sending')
              : t('forgotPassword.buttons.submit')}
          </button>
        </form>
      </div>
    </div>
  );
};

export default ForgotPassword;
