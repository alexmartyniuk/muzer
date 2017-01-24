package controllers

// ClientError is a generic error specific to the 'controllers' package.
type ClientError struct {
	msg string
}

// Error returns a string representation of the error condition.
func (self ClientError) Error() string {
	return self.msg
}
