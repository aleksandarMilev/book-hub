import { Component, type ReactNode } from 'react';

import errorIllustration from './err-b.webp';

type Props = {
  children: ReactNode;
  fallback?: ReactNode;
  onError?: (error: unknown, info: string) => void;
  onReset?: () => void;
};

type State = {
  hasError: boolean;
  error: unknown;
};

export default class ErrorBoundary extends Component<Props, State> {
  override state: State = { hasError: false, error: null };

  static getDerivedStateFromError(error: unknown): State {
    return { hasError: true, error };
  }

  override componentDidCatch(error: unknown, info: unknown) {
    if (this.props.onError) {
      this.props.onError(error, JSON.stringify(info));
    } else {
      console.error('ErrorBoundary caught an error:', error, info);
    }
  }

  resetError = () => {
    this.setState({ hasError: false, error: null });

    if (this.props.onReset) {
      this.props.onReset();
    }
  };

  renderFallback() {
    if (this.props.fallback) {
      return this.props.fallback;
    }

    return (
      <div style={fallbackStyles.container}>
        <img src={errorIllustration} alt="Something went wrong" style={fallbackStyles.image} />
        <h2 style={fallbackStyles.title}>Something went wrong</h2>
        <p style={fallbackStyles.text}>
          An unexpected error occurred. Please try again. If the problem persists, you can refresh
          the page or come back later.
        </p>
        <button style={fallbackStyles.button} onClick={this.resetError}>
          Try Again
        </button>
      </div>
    );
  }

  override render() {
    if (this.state.hasError) {
      return this.renderFallback();
    }

    return this.props.children;
  }
}

const fallbackStyles = {
  container: {
    padding: '40px',
    textAlign: 'center' as const,
    fontFamily: 'Inter, sans-serif',
    display: 'flex',
    flexDirection: 'column' as const,
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: '60vh',
  },
  image: {
    maxWidth: '360px',
    width: '100%',
    marginBottom: '24px',
    borderRadius: '18px',
    boxShadow: '0 12px 30px rgba(0, 0, 0, 0.18)',
  },
  title: {
    fontSize: '1.9rem',
    fontWeight: 700,
    marginBottom: '12px',
    color: '#1b1b1b',
  },
  text: {
    fontSize: '1rem',
    color: '#555',
    marginBottom: '22px',
    maxWidth: '480px',
  },
  button: {
    padding: '10px 24px',
    background: 'linear-gradient(90deg, #6ea8fe, #9a6bff)',
    border: 'none',
    borderRadius: '999px',
    fontSize: '1rem',
    fontWeight: 600,
    color: 'white',
    cursor: 'pointer',
    boxShadow: '0 8px 20px rgba(154, 107, 255, 0.4)',
  },
};
